namespace ThaiTuanERP2025.Application.Shared.Interfaces
{
	public interface IRealtimeNotifier
	{
		#region Notifications
		Task PushNotificationsAsync(IEnumerable<Guid> userIds, IEnumerable<object> payloads, CancellationToken cancellationToken = default);
		Task PushNotificationReadAsync(Guid notificationId, Guid receiverId, DateTime readAt, CancellationToken cancellationToken = default);
		#endregion

		#region Reminder
		Task PushRemindersAsync(IEnumerable<Guid> userIds, IEnumerable<object> payloads, CancellationToken cancellationToken = default);
		Task PushReminderResolvedAsync(Guid userId, Guid reminderId, CancellationToken cancellationToken);
		#endregion
	}
}
