namespace ThaiTuanERP2025.Application.Notifications.Services
{
	public interface IRealtimeNotifier
	{
		Task NotifyStepActivatedAsync(IReadOnlyCollection<Guid> targetUserIds, IReadOnlyCollection<object> payloads, CancellationToken cancellationToken = default);
		Task PushRemindersAsync(IEnumerable<Guid> userIds, IEnumerable<object> payloads, CancellationToken cancellationToken = default);
		Task PushRemindersResolvedAsync(IEnumerable<Guid> userIds, IEnumerable<Guid> alarmIds, CancellationToken cancellationToken = default);
	}
}
