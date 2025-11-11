using MediatR;
using ThaiTuanERP2025.Application.Core.Services;
using ThaiTuanERP2025.Domain.Core.Enums;
using ThaiTuanERP2025.Domain.Finance.Events;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans.EventHandlers
{
	public sealed class BudgetPlanCreatedEventHandler : INotificationHandler<BudgetPlanCreatedEvent>
	{
		private readonly INotificationService _notification;
		private readonly IReminderService _reminder;
		public BudgetPlanCreatedEventHandler(INotificationService notification, IReminderService reminder) {
			  _notification = notification;	
			  _reminder = reminder;
		}

		public async Task Handle(BudgetPlanCreatedEvent notification, CancellationToken cancellationToken)
		{
			var message = $"Bạn được giao xem xét kế hoạch ngân sách mới (ID: {notification.BudgetPlanId}). Hạn xử lý: {notification.DueAt:HH:mm dd/MM/yyyy}.";

			// Gửi thông báo đến Reviewer
			await _notification.SendAsync(
				userId: notification.ReviewerUserId,
				title: "Kế hoạch ngân sách mới chờ xem xét",
				message: message,
				null,
				NotificationType.Task,
				cancellationToken
			);

			// Đặt nhắc việc (trước hạn 1 tiếng)
			await _reminder.ScheduleReminderAsync(
				userId: notification.ReviewerUserId,
				subject: "Xem xét kế hoạch ngân sách",
				message: $"Kế hoạch {notification.BudgetPlanId} sắp đến hạn xử lý.",
				triggerAt: notification.DueAt.AddHours(-1),
				null,
				cancellationToken
			);
		}
	}
}
