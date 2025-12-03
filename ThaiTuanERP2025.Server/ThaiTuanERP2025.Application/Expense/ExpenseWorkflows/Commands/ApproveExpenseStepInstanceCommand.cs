using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Core.Followers;
using ThaiTuanERP2025.Application.Core.Notifications;
using ThaiTuanERP2025.Application.Core.Reminders;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Repositories;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Core.Enums;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Shared.Enums;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Commands
{
	public sealed record ApproveExpenseStepInstanceCommand(Guid WorkflowInstanceId) : IRequest<Unit>;

	public sealed class ApproveExpenseStepInstanceCommandHandler : IRequestHandler<ApproveExpenseStepInstanceCommand, Unit>
	{
		private readonly IUnitOfWork _uow;
		private readonly ICurrentUserService _currentUser;
		private readonly INotificationService _notificationService;
		private readonly IReminderService _reminderService;
		private readonly IExpensePaymentReadRepository _expensePaymentRepo;
		private readonly IFollowerService _followerService;
		private readonly IUserReminderReadRepository _reminderRepo;
		private readonly IUserNotificationReadRepository _notificationRepo;
		public ApproveExpenseStepInstanceCommandHandler(
			IUnitOfWork uow, ICurrentUserService currentUser, INotificationService notificationSerivce, IReminderService reminderService,
			IExpensePaymentReadRepository expensePaymentRepo, IFollowerService followerService, IUserReminderReadRepository reminderRepo,
			IUserNotificationReadRepository notificationRepo
		) {
			_uow = uow;
			_currentUser = currentUser;
			_reminderService = reminderService;
			_notificationService = notificationSerivce;
			_expensePaymentRepo = expensePaymentRepo;
			_followerService = followerService;	
			_reminderRepo = reminderRepo;
			_notificationRepo = notificationRepo;
		}
			
		public async Task<Unit> Handle(ApproveExpenseStepInstanceCommand command, CancellationToken cancellationToken)
		{
			// === Validate ====
			var userId = _currentUser.UserId ?? throw new AppException("Tài khoản của bạn không hợp lệ");
			var userExist = await _uow.Users.ExistAsync(q => q.Id == userId && q.IsActive, cancellationToken);
			if (!userExist) throw new UnauthorizedException("Tài khoản của bạn không hợp lệ");

			var workflowInstance = await _uow.ExpenseWorkflowInstances.SingleOrDefaultIncludingAsync(
				q => q.Id == command.WorkflowInstanceId,
				includes: x => x.Steps,
				asNoTracking: false,
				cancellationToken: cancellationToken
			) ?? throw new NotFoundException("Không tìm thấy workflow");

			var currentStep = workflowInstance.GetCurrentStep();

                        var approverIds = currentStep.GetResolvedApproverIds();
			if (!approverIds.Contains(userId))
				throw new BusinessRuleViolationException("Bạn không có quyền duyệt");

			// ==== APPROVE ====
			currentStep.Approve(userId);
                        if (currentStep.IsFullyApproved())
                        {
                                workflowInstance.ActivateNextStep();
                        }

			var (creatorId, paymentName) = await LoadCreatorAndNameAsync(
				workflowInstance.DocumentId,
				cancellationToken
			);

			// resolve previous approvers reminder / mark notifications as read
			await ResolveCurrentApproverWorkAsync(workflowInstance.DocumentId, approverIds, cancellationToken);

			// === IF WORKFLOW INSTANCE HAS BEEN APPROVED ====
			if (workflowInstance.Status == Domain.Expense.Enums.ExpenseWorkflowStatus.Approved)
			{
				
				
				var lastStep = workflowInstance.GetLastStep();
				if(lastStep.ApprovedBy is null)
					throw new AppException("Không tìm thấy thông tin người duyệt bước cuối cùng");

				await _notificationService.SendAsync(
					senderId: lastStep.ApprovedBy.Value,
					receiverId: creatorId,
					title: $"Thanh toán {paymentName} đã được phê duyệt",
					message: $"Thanh toán {paymentName} đã được duyệt",
					linkType: Domain.Core.Enums.LinkType.ExpensePaymentDetail,
					targetId: workflowInstance.DocumentId,
					type: Domain.Core.Enums.NotificationType.Task,
					cancellationToken: cancellationToken
				);

				await _uow.SaveChangesAsync(cancellationToken);
				return Unit.Value;
			}

			// ==== IF WORKFLOW INSTANCE NOT APPROVED YET ===
			var expensePaymentName = await _expensePaymentRepo.GetNameAsync(workflowInstance.DocumentId, cancellationToken);
			var message = $"Thanh toán {expensePaymentName} đang chờ bạn duyệt";
			var subject = $"Duyệt thanh toán {expensePaymentName}";

                        var nextStep = workflowInstance.GetCurrentStep();
                        if (nextStep.DueAt is null)
                                throw new AppException("Step hiện tại chưa có hạn xử lý (DueAt = null).");


			// ==== REMINDER ====
			var nextApproverIds = nextStep.GetResolvedApproverIds();
			// set reminder for next approver
			await _reminderService.ScheduleReminderManyAsync(
				userIds: nextApproverIds,
				subject: subject,
				message: message,
				slaHours: nextStep.SlaHours,
				dueAt: nextStep.DueAt.Value,
				linkType: Domain.Core.Enums.LinkType.ExpensePaymentDetail,
				targetId: workflowInstance.DocumentId,
				cancellationToken
			);

			// ==== NOTIFICATIONS ====

			// send notifications
			await _notificationService.SendToManyAsync(
				senderId: creatorId,
				nextApproverIds,
				title: subject,
				message: message,
				linkType: Domain.Core.Enums.LinkType.ExpensePaymentDetail,
				targetId: workflowInstance.DocumentId,
				type: Domain.Core.Enums.NotificationType.Task,
				cancellationToken: cancellationToken
			);

			// ==== FOLLOWERS ====
			// Set Follow for next approver
			await _followerService.FollowManyAsync(DocumentType.ExpensePayment, workflowInstance.DocumentId, nextApproverIds, cancellationToken);

                        await _uow.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		private async Task ResolveCurrentApproverWorkAsync(Guid documentId, IReadOnlyCollection<Guid> approverIds, CancellationToken cancellationToken)
		{
			// Resolve reminders
			var resolveReminderIds = await _reminderRepo.ListProjectedAsync(
				q => q.Where(x => approverIds.Contains(x.UserId) && x.TargetId == documentId && !x.IsResolved)
					.Select(x => x.Id),
				cancellationToken: cancellationToken
			) ?? new List<Guid>();

			if (resolveReminderIds.Any())
				await _reminderService.MarkResolvedManyAsync(resolveReminderIds, cancellationToken);

			var taskNotificationIds = await _notificationRepo.ListProjectedAsync(
				q => q.Where(x => approverIds.Contains(x.ReceiverId)
					&& x.TargetId == documentId
					&& x.Type == NotificationType.Task
					&& !x.IsRead
				).Select(x => x.Id),
				cancellationToken: cancellationToken
			) ?? new List<Guid>();

			if (taskNotificationIds.Any())
				await _notificationService.MarkAsReadManyAsync(taskNotificationIds, cancellationToken);
		}

		private async Task<(Guid creatorId, string paymentName)>LoadCreatorAndNameAsync(Guid documentId, CancellationToken cancellationToken)
		{
			var creatorId = await _expensePaymentRepo.GetCreatorIdAsync(documentId, cancellationToken)
				?? throw new NotFoundException("Không tìm thấy thông tin người tạo thanh toán này");

			var paymentName = await _expensePaymentRepo.GetNameAsync(documentId, cancellationToken)
				?? throw new NotFoundException("Không tìm thấy tên thanh toán");

			return (creatorId, paymentName);
		}
	}

	public sealed class ApproveExpenseStepInstanceCommandValidator : AbstractValidator<ApproveExpenseStepInstanceCommand>
	{
		public ApproveExpenseStepInstanceCommandValidator()
		{
			RuleFor(x => x.WorkflowInstanceId).NotEmpty().WithMessage("Định danh bước duyệt không được để trống");
		}
	}

	
}
