using MediatR;
using ThaiTuanERP2025.Application.Core.Notifications;
using ThaiTuanERP2025.Application.Core.Reminders;
using ThaiTuanERP2025.Application.Finance.BudgetPeriods;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Domain.Core.Enums;
using ThaiTuanERP2025.Domain.Finance.Events;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans.EventHandlers
{
	public sealed class BudgetPlanApprovedEventHandler : INotificationHandler<BudgetPlanApprovedEvent>
	{
		private readonly INotificationService _notification;
		private readonly IReminderService _reminder;
		private readonly IBudgetPeriodReadRepository _budgetPeriodRepo;
		private readonly IUnitOfWork _uow;
		public BudgetPlanApprovedEventHandler(INotificationService notification, IReminderService reminder, IBudgetPeriodReadRepository budgetPeriodRepo, IUnitOfWork uow)
		{
			_notification = notification;
			_reminder = reminder;
			_budgetPeriodRepo = budgetPeriodRepo;
			_uow = uow;
		}

		public async Task Handle(BudgetPlanApprovedEvent domainEvent, CancellationToken cancellationToken)
		{
			var budgetPeriod = await _budgetPeriodRepo.GetByIdProjectedAsync(domainEvent.BudgetPlan.BudgetPeriodId, cancellationToken);
			if (budgetPeriod is null)
				throw new KeyNotFoundException($"Không tìm thấy kỳ ngân sách");

			var budgetPlanName = $"{budgetPeriod.Month}/{budgetPeriod.Year}";

			var approverId = domainEvent.BudgetPlan.ApprovedByUserId!.Value;
			var creatorId = domainEvent.BudgetPlan.CreatedByUserId;
			var reviewerId = domainEvent.BudgetPlan.ReviewedByUserId;

			// resolve approver reminder
			var reminder = await _uow.UserReminders.SingleOrDefaultAsync(
				q => q.Where(
					x => x.UserId == domainEvent.BudgetPlan.ApprovedByUserId
						&& x.TargetId == domainEvent.BudgetPlan.Id
						&& !x.IsResolved
				),
				asNoTracking: false,
				cancellationToken: cancellationToken
			) ?? throw new NotFoundException("Không tìm thấy nhắc việc của người duyệt");
			await _reminder.MarkResolvedAsync(reminder.Id, cancellationToken);

			// mark approver's task notification as read
			var approverTaskNotification = await _uow.UserNotifications.SingleOrDefaultAsync(
				q => q.Where(
					x => x.ReceiverId == approverId 
					&& x.TargetId.Equals(domainEvent.BudgetPlan.Id)
					&& x.Type == NotificationType.Task
					&& !x.IsRead
				),
				asNoTracking: false,
				cancellationToken: cancellationToken
			);
			if (approverTaskNotification is not null)
				await _notification.MarkAsReadAsync(approverTaskNotification.Id, cancellationToken);

			// Send notifications to creator and reviewer
			await _notification.SendToManyAsync(
				approverId,
				[creatorId!.Value, reviewerId!.Value],
				$"Kế hoạch ngân sách {budgetPlanName} đã được phê duyệt",
				$"Kế hoạch ngân sách {budgetPlanName} đã được phê duyệt",
				LinkType.BudgetPlanApproved,
				domainEvent.BudgetPlan.Id,
				NotificationType.Info,
				cancellationToken: cancellationToken
			);
		}
	}
}
