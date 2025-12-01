using MediatR;
using System.Net.WebSockets;
using ThaiTuanERP2025.Application.Account.Users.Repositories;
using ThaiTuanERP2025.Application.Core.Notifications;
using ThaiTuanERP2025.Application.Core.Reminders;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Repositories;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Finance.Enums;
using ThaiTuanERP2025.Domain.Finance.Events.BudgetTransactions;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Commands
{
	public sealed record RejectExpenseStepInstanceCommand(Guid WorkflowInstanceId) : IRequest<Unit>;

	public sealed class RejectExpenseStepInstanceCommandHandler : IRequestHandler<RejectExpenseStepInstanceCommand, Unit>
	{
		private readonly IUnitOfWork _uow;
		private readonly ICurrentUserService _currentUser;
		private readonly INotificationService _notificationService;
		private readonly IReminderService _reminderService;
		private readonly IExpensePaymentReadRepository _expensePaymentRepo;
		private readonly IUserReminderReadRepository _reminderRepo;
		private readonly IUserNotificationReadRepository _notificationRepo;
		private readonly IUserReadRepostiory _userRepo;
		public RejectExpenseStepInstanceCommandHandler(
			IUnitOfWork uow, ICurrentUserService currentUser, 
			INotificationService notificationService, IReminderService reminderSerivce,
			IExpensePaymentReadRepository expensePaymentRepo,
			IUserReminderReadRepository reminderRepo, IUserNotificationReadRepository notificationRepo,
			IUserReadRepostiory userRepo
		) { 
			_uow = uow;
			_currentUser = currentUser;
			_notificationService = notificationService;
			_reminderService = reminderSerivce;
			_expensePaymentRepo = expensePaymentRepo;
			_notificationRepo = notificationRepo;
			_reminderRepo = reminderRepo;
			_userRepo = userRepo;
		}

		public async Task<Unit> Handle(RejectExpenseStepInstanceCommand command, CancellationToken cancellationToken)
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

			var expensePayment = await _uow.ExpensePayments.SingleOrDefaultIncludingAsync(
				predicate: x => x.Id == workflowInstance.DocumentId,
				includes: x => x.Items,
				asNoTracking: false,
				cancellationToken: cancellationToken
			) ?? throw new NotFoundException("Không tìm thấy thanh toán yêu cầu");

			var currentStep = workflowInstance.GetCurrentStep();
			var approverIds = currentStep.GetResolvedApproverIds();
			if (!approverIds.Contains(userId))
				throw new BusinessRuleViolationException("Bạn không có quyền duyệt");

			workflowInstance.MarkRejected(userId);
			expensePayment.MarkRejected();

			// ==== RECORD BUDGET TRANSACTION =====
			foreach(var item in expensePayment.Items)
			{
				var debitTransactions = await _uow.BudgetTransactions.ListAsync(
					q => q.Where(x =>
						x.ExpensePaymentItemId == item.Id &&
						x.Type == BudgetTransactionType.Debit
					),
					asNoTracking: false,
					cancellationToken: cancellationToken
				);

                                foreach (var debit in debitTransactions)
                                {
                                        // Tạo transaction Credit để đảo ngược
                                        var reverse = debit.CreateReverse();

                                        await _uow.BudgetTransactions.AddAsync(reverse, cancellationToken);
                                }
                        }

			// === AFTER REJECT ===
			// Reminders
			// resolve reminder for current approver
			var resolveReminderIds = await _reminderRepo.ListProjectedAsync(
				q => q.Where(
					x => approverIds.Contains(x.UserId)
						&& x.TargetId == workflowInstance.DocumentId
						&& !x.IsResolved
				).Select(x => x.Id),
				cancellationToken: cancellationToken
			) ?? throw new NotFoundException("Không tìm thấy nhắc việc của người duyệt");
			await _reminderService.MarkResolvedManyAsync(resolveReminderIds, cancellationToken);

			// Notifications
			var creatorId = await _expensePaymentRepo.GetCreatorIdAsync(workflowInstance.DocumentId, cancellationToken)
				?? throw new NotFoundException("Không tìm thấy thông tin người tạo thanh toán này");

			// mark as read
			var taskNotificaitonIds = await _notificationRepo.ListProjectedAsync(
				q => q.Where(x =>
					approverIds.Contains(x.ReceiverId)
					&& x.TargetId == workflowInstance.DocumentId
					&& x.Type == Domain.Core.Enums.NotificationType.Task
					&& !x.IsRead
				).Select(x => x.Id),
				cancellationToken: cancellationToken
			);
			if (taskNotificaitonIds.Any())
				await _notificationService.MarkAsReadManyAsync(taskNotificaitonIds, cancellationToken);

			// Send notification to creator
			var userName = await _userRepo.GetUserNameAsync(userId, cancellationToken);
			var managerApproverId = await _expensePaymentRepo.GetManagerApproverId(expensePayment.Id, cancellationToken);

			var receivers = new List<Guid> { creatorId, managerApproverId }.Where(id => id != Guid.Empty).Distinct().ToList();
			await _notificationService.SendToManyAsync(
				senderId: userId,
				userIds: receivers,
				title: $"{expensePayment.Name} đã bị {userName} từ chối",
				message: $"Thanh toán {expensePayment.Name} đã bị {userName} từ chối",
				linkType: Domain.Core.Enums.LinkType.ExpensePaymentDetail,
				expensePayment.Id,
				type: Domain.Core.Enums.NotificationType.Info,
				cancellationToken: cancellationToken
			);

			await _uow.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
