using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Utils;
using ThaiTuanERP2025.Application.Expense.Services.ApprovalWorkflows;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.RejectCurrentStep
{
	public sealed class RejectCurrentStepHandler : IRequestHandler<RejectCurrentStepCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IApprovalStepService _approvalStepService;
		public RejectCurrentStepHandler(IUnitOfWork unitOfWork, IApprovalStepService approvalStepService)
		{
			_unitOfWork = unitOfWork;
			_approvalStepService = approvalStepService;
		}

		public async Task<Unit> Handle(RejectCurrentStepCommand command, CancellationToken cancellationToken) {
			var workflowInstance = await _unitOfWork.ApprovalWorkflowInstances.SingleOrDefaultIncludingAsync(
				x => x.Id == command.WorkflowInstanceId,
				includes: x => x.Steps,
				asNoTracking: false,
				cancellationToken: cancellationToken
			) ?? throw new NotFoundException("Không tìm thấy workflow instance");

			if(workflowInstance.Status is not WorkflowStatus.InProgress) 
				throw new ConflictException($"Workflow ở trạng thái {workflowInstance.Status}, không thể từ chối bước duyệt");

			var paymentDetail = await _unitOfWork.ExpensePayments.GetByIdAsync(command.PaymentId)
				?? throw new NotFoundException("Không tìm thấy chi phí thanh toán");

			// current step
			var currentStep = workflowInstance.Steps.OrderBy(s => s.Order)
				.FirstOrDefault(s => s.Order == workflowInstance.CurrentStepOrder)
				?? workflowInstance.Steps.OrderBy(s => s.Order).FirstOrDefault();
			if (currentStep is null)
				throw new ConflictException("Workflow không có step hiện tại");
			if (currentStep.Id != command.StepInstanceId)
				throw new ConflictException("Chỉ được duyệt step đang hoạt động");
			if (currentStep.Status is not Domain.Expense.Enums.StepStatus.Waiting)
				throw new ConflictException($"Step hiện tại không ở trạng thái chờ duyệt, trạng thái hiện tại: {currentStep.Status}");

			// authorize
			var userId = command.UserId;
			var candidates = JsonUtils.ParseGuidArray(currentStep.ResolvedApproverCandidatesJson).ToHashSet();
			if (currentStep.FlowType == FlowType.Single) {
				if (currentStep.DefaultApproverId != userId)
					throw new ForbiddenException("Bạn không phải người duyệt được chỉ định ở bước này.");
			} else {
				if (!candidates.Contains(userId))
					throw new ForbiddenException("Bạn không có quyền duyệt ở bước này");
			}

			// Reject step
			var now = DateTime.UtcNow;
			currentStep.Reject(userId, command.Comment, now);

			// Finish workflow & payment
			workflowInstance.SetStatus(WorkflowStatus.Rejected);
			if(workflowInstance.DocumentType?.Equals("ExpensePayment", StringComparison.OrdinalIgnoreCase) == true) {
				var payment = await _unitOfWork.ExpensePayments.GetByIdAsync(workflowInstance.DocumentId);
				if (payment is not null)
					payment.Reject();
			}

			await _approvalStepService.PublishAsync(workflowInstance, currentStep, docName: paymentDetail.Name, docId: paymentDetail.Id, docType: "ExpensePayment", cancellationToken);

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
