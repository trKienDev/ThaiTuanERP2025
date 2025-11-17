using MediatR;
using ThaiTuanERP2025.Application.Core.Notifications;
using ThaiTuanERP2025.Application.Core.Reminders;
using ThaiTuanERP2025.Application.Finance.BudgetPeriods;
using ThaiTuanERP2025.Domain.Core.Enums;
using ThaiTuanERP2025.Domain.Finance.Events;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans.EventHandlers
{
	public sealed class BudgetPlanCreatedEventHandler : INotificationHandler<BudgetPlanCreatedEvent>
	{
		private readonly INotificationService _notification;
		private readonly IReminderService _reminder;
		private readonly IBudgetPeriodReadRepository _budgetPeriodRepo;
		public BudgetPlanCreatedEventHandler(INotificationService notification, IReminderService reminder, IBudgetPeriodReadRepository budgetPeriodRepo)
		{
			_notification = notification;
			_reminder = reminder;
			_budgetPeriodRepo = budgetPeriodRepo;
		}

		public async Task Handle(BudgetPlanCreatedEvent domainEvent, CancellationToken cancellationToken) {
			var budgetPeriod = await _budgetPeriodRepo.GetByIdProjectedAsync(domainEvent.BudgetPlan.BudgetPeriodId, cancellationToken);
			if (budgetPeriod is null)
				throw new KeyNotFoundException($"Không tìm thấy kỳ ngân sách");

			var budgetPlanName = $"{budgetPeriod.Month}/{budgetPeriod.Year}";

			var message = $"kế hoạch ngân sách {budgetPlanName} đang chờ bạn xem xét";

			// Gửi thông báo đến Reviewer
			await _notification.SendAsync(
				senderId: domainEvent.BudgetPlan.CreatedByUserId!.Value,
				receiverId: domainEvent.ReviewerUserId,
				title: "Kế hoạch ngân sách mới chờ xem xét",
				message: message,
				linkType: LinkType.BudgetPlanReview,
				targetId: domainEvent.BudgetPlanId,
				type: NotificationType.Task,
				cancellationToken: cancellationToken
			);

			// Đặt nhắc việc (trước hạn 1 tiếng)
			await _reminder.ScheduleReminderAsync(
				userId: domainEvent.ReviewerUserId,
				subject: "Xem xét kế hoạch ngân sách",
				message: $"Kế hoạch ngân sách {budgetPlanName} cần bạn xem xét.",
				slaHours: 8,
				dueAt: domainEvent.DueAt,
				linkType: LinkType.BudgetPlanReview,
				targetId: domainEvent.BudgetPlanId,	
				cancellationToken
			);
		}
	}
}
