namespace ThaiTuanERP2025.Application.Common.Interfaces
{
	public interface IRealtimeNotifier
	{
		Task PushNotificationsAsync(IEnumerable<Guid> userIds, IEnumerable<object> payloads, CancellationToken cancellationToken = default);
		Task PushRemindersAsync(IEnumerable<Guid> userIds, IEnumerable<object> payloads, CancellationToken cancellationToken = default);
	}
}
