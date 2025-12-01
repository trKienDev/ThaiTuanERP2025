using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Core.Notifications;
using ThaiTuanERP2025.Application.Core.Reminders;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Repositories;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Exceptions;
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
		public ApproveExpenseStepInstanceCommandHandler(
			IUnitOfWork uow, ICurrentUserService currentUser, INotificationService notificationSerivce, IReminderService reminderService,
			IExpensePaymentReadRepository expensePaymentRepo
		) {
			_uow = uow;
			_currentUser = currentUser;
			_reminderService = reminderService;
			_notificationService = notificationSerivce;
			_expensePaymentRepo = expensePaymentRepo;
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

			currentStep.Approve(userId);
                        if (currentStep.IsFullyApproved())
                        {
                                workflowInstance.ActivateNextStep();
                        }

			var expensePaymentName = await _expensePaymentRepo.GetNameAsync(workflowInstance.DocumentId, cancellationToken);
			var message = $"Thanh toán {expensePaymentName} đang chờ bạn duyệt";
			var subject = $"Duyệt thanh toán {expensePaymentName}";

                        var nextStep = workflowInstance.GetCurrentStep();
                        if (nextStep.DueAt is null)
                                throw new AppException("Step hiện tại chưa có hạn xử lý (DueAt = null).");

                        var nextApproverIds = nextStep.GetResolvedApproverIds();
			// set reminder for next approver
			await _reminderService.ScheduleReminderManyAsync(
				userIds: nextApproverIds,
				subject: subject,
				message: message,
				slaHours: nextStep.SlaHours,
				dueAt: nextStep.DueAt.Value,
				linkType: Domain.Core.Enums.LinkType.ExpensePaymentApprove,
				targetId: workflowInstance.DocumentId,
				cancellationToken
			);

			var creatorId = await _expensePaymentRepo.GetCreatorIdAsync(workflowInstance.DocumentId, cancellationToken)
				?? throw new NotFoundException("Không tìm thấy thông tin người tạo thanh toán này");

			// send notifications
			await _notificationService.SendToManyAsync(
				senderId: creatorId,
				nextApproverIds,
				title: subject,
				message: message,
				linkType: Domain.Core.Enums.LinkType.ExpensePaymentApprove,
				targetId: workflowInstance.DocumentId,
				type: Domain.Core.Enums.NotificationType.Task,
				cancellationToken: cancellationToken
			);

                        await _uow.SaveChangesAsync(cancellationToken);
			return Unit.Value;
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
