using MediatR;
using ThaiTuanERP2025.Application.Core.Notifications;
using ThaiTuanERP2025.Application.Core.Reminders;
using ThaiTuanERP2025.Application.Finance.BudgetPeriods;
using ThaiTuanERP2025.Domain.Core.Enums;
using ThaiTuanERP2025.Domain.Finance.Events;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans.EventHandlers
{
	public sealed class BudgetPlanReviewedEventHandler : INotificationHandler<BudgetPlanReviewedEvent>
	{
		private readonly INotificationService _notification;
		private readonly IReminderService _reminder;
		private readonly IBudgetPeriodReadRepository _budgetPeriodRepo;
		public BudgetPlanReviewedEventHandler(INotificationService notification, IReminderService reminder, IBudgetPeriodReadRepository budgetPeriodRepo)
		{
			_notification = notification;
			_reminder = reminder;
			_budgetPeriodRepo = budgetPeriodRepo;
		}

		public async Task Handle(BudgetPlanReviewedEvent domainEvent, CancellationToken cancellationToken) {
			var budgetPeriod = await _budgetPeriodRepo.GetByIdProjectedAsync(domainEvent.BudgetPlan.BudgetPeriodId, cancellationToken);
			if (budgetPeriod is null)
				throw new KeyNotFoundException($"Không tìm thấy kỳ ngân sách");

			var budgetPlanName = $"{budgetPeriod.Month}/{budgetPeriod.Year}";

			var message = $"kế hoạch ngân sách {budgetPlanName} đang chờ bạn phê duyệt";

			await _notification.SendAsync(
				senderId: domainEvent.BudgetPlan.CreatedByUserId!.Value,
				receiverId: domainEvent.ApproverUserId,
				title: "Kế hoạch ngân sách mới chờ phê duyệt",
				message: message,
				linkType: LinkType.BudgetPlanReview,
				targetId: domainEvent.BudgetPlan.Id,
				type: NotificationType.Task,
				cancellationToken: cancellationToken
			);

			// Đặt nhắc việc (trước hạn 1 tiếng)
			await _reminder.ScheduleReminderAsync(
				userId: domainEvent.ApproverUserId,
				subject: $"Phê duyệt kế hoạch ngân sách {budgetPlanName}",
				message: message,
				slaHours: 8,
				dueAt: domainEvent.DueAt,
				linkType: LinkType.BudgetPlanReview,
				targetId: domainEvent.BudgetPlan.Id,
				cancellationToken
			);
		}
	}
}
