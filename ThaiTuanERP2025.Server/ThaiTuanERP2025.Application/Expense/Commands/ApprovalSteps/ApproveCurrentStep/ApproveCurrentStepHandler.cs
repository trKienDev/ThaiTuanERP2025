using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Utils;
using ThaiTuanERP2025.Application.Expense.Services.ApprovalWorkflows;
using ThaiTuanERP2025.Application.Notifications.Services;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.ApproveCurrentStep
{
	public sealed class ApproveCurrentStepHandler : IRequestHandler<ApproveCurrentStepCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ITaskReminderService _taskReminderService;
		private readonly INotificationService _notificationService;
		private readonly IApprovalStepService _approvalStepService;
		private readonly IRealtimeNotifier _realtimeNotifier;
		public ApproveCurrentStepHandler(
			IUnitOfWork unitOfWork, ITaskReminderService taskReminderService, INotificationService notificationService,
			IApprovalStepService approvalStepService, IRealtimeNotifier realtimeNotifier
		)
		{
			_unitOfWork = unitOfWork;
			_taskReminderService = taskReminderService;
			_notificationService = notificationService;
			_approvalStepService = approvalStepService;
			_realtimeNotifier = realtimeNotifier;
		}

		public async Task<Unit> Handle(ApproveCurrentStepCommand command, CancellationToken cancellationToken)
		{
			// 1 ) Load instance + steps
			var workflowInstance = await _unitOfWork.ApprovalWorkflowInstances.SingleOrDefaultIncludingAsync(
				x => x.Id == command.WorkflowInstanceId,
				includes: x => x.Steps,
				asNoTracking: false,
				cancellationToken: cancellationToken
			) ?? throw new NotFoundException("Không tìm thấy workflow instance");

			if (workflowInstance.Status is not Domain.Expense.Enums.WorkflowStatus.InProgress)
				throw new ConflictException($"Workflow ở trạng thái {workflowInstance.Status}, không thể duyệt bước duyệt");

			var paymentDetail = await _unitOfWork.ExpensePayments.GetByIdAsync(command.PaymentId)
				?? throw new NotFoundException("Không tìm thấy chi phí thanh toán");

			var approverUser = await _unitOfWork.Users.GetByIdAsync(command.UserId)
				?? throw new NotFoundException("Không tìm thấy thông tin người duyệt");

			// current step
			var currentStep = workflowInstance.Steps.OrderBy(s => s.Order)
				.FirstOrDefault(s => s.Order == workflowInstance.CurrentStepOrder)
				?? workflowInstance.Steps.OrderBy(s => s.Order).FirstOrDefault();
			if (currentStep is null)
				throw new ConflictException("Workflow không có step hiện tại");
			if (currentStep.Id != command.StepInstanceId)
				throw new ConflictException("Chỉ được duyệt step đang hoạt động");
			if(currentStep.Status is not Domain.Expense.Enums.StepStatus.Waiting)
				throw new ConflictException($"Step hiện tại không ở trạng thái chờ duyệt, trạng thái hiện tại: {currentStep.Status}");

			// 2 ) Authorize — kiểm tra SelectedApprover hoặc nằm trong danh sách candidates
			//    - FlowType.Single: bắt buộc SelectedApproverId == user
			//    - FlowType.OneOfN: nếu user thuộc candidates và khác SelectedApproverId → set SelectedApproverId = user
			var userId = command.UserId;
			// parse candidates
			var candidates = JsonUtils.ParseGuidArray(currentStep.ResolvedApproverCandidatesJson).ToHashSet();
			if (currentStep.FlowType == Domain.Expense.Enums.FlowType.Single)
			{
				if(!candidates.Contains(userId))
					throw new ForbiddenException("Bạn không có quyền duyệt bước này");
			}
			else if (currentStep.FlowType == Domain.Expense.Enums.FlowType.OneOfN)
			{
				var inCandidates = candidates.Contains(userId);
				if(!inCandidates)
					throw new ForbiddenException("Bạn không có quyền duyệt bước này");
				if (currentStep.SelectedApproverId != userId)
				{
					// cập nhật người chịu trách nhiệm cuối cùng
					typeof(ApprovalStepInstance).GetProperty(nameof(ApprovalStepInstance.SelectedApproverId))!
						.SetValue(currentStep, userId);
				}
			}
			else
			{
				throw new ConflictException("FlowType không hợp lệ");
			}

			// 3 ) Approve step
			var now = DateTime.UtcNow;
			currentStep.Approve(userId, now);
			// append history step-level (nếu cần)
			// Resolve current step reminders
			await _taskReminderService.ResolveByStepAsync(currentStep.Id, "Approved" ,cancellationToken);

			// 4 ) Activate next step or complete workflow
			var ordered = workflowInstance.Steps.OrderBy(s => s.Order).ToList();
			var idx = ordered.FindIndex(s => s.Id == currentStep.Id);
			var nextStep = (idx >= 0 && idx + 1 < ordered.Count) ? ordered[idx + 1] : null;
			if(nextStep is not null) {
				// activate next step + set current step
				nextStep.Activate(now);
				workflowInstance.SetCurrentStep(nextStep.Order);

				// send notification + create reminder for next step
				await _approvalStepService.PublishAsync(workflowInstance, nextStep, paymentDetail.Name, paymentDetail.Id, "ExpensePayment", cancellationToken);
			} else {
				// complete workflow
				workflowInstance.MarkApproved("All step approved");
				if(workflowInstance.DocumentType.Equals("ExpensePayment", StringComparison.OrdinalIgnoreCase) == true) {
					var payment = await _unitOfWork.ExpensePayments.GetByIdAsync(workflowInstance.DocumentId);
					if (payment != null)
					{
						payment.ReadyForOutgoingPayment();
					}

					await _notificationService.NotifyWorkflowApprovedAsync(
						workflowInstance, currentStep,
						targetUserIds: new[] { workflowInstance.CreatedByUserId },
						approver: approverUser.FullName,
						docName: paymentDetail.Name,
						documentId: paymentDetail.Id,
						documentType: "ExpensePayment",
						cancellationToken
					);
				}
			}

			var reminder = await _unitOfWork.TaskReminders.SingleOrDefaultAsync(
				q => q.Where(t => t.WorkflowInstanceId == workflowInstance.Id &&
					!t.IsResolved && t.StepInstanceId == currentStep.Id && t.UserId == userId
				),
				cancellationToken: cancellationToken,
				asNoTracking: false
			); 
			if(reminder is not null) {
				reminder.Resolve("");
				await _realtimeNotifier.PushRemindersResolvedAsync(candidates, new[] { reminder.Id }, cancellationToken);
			}
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			
			return Unit.Value;
		}
	}
}
