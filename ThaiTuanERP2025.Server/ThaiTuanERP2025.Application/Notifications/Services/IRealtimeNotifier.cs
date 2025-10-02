namespace ThaiTuanERP2025.Application.Notifications.Services
{
	public interface IRealtimeNotifier
	{
		Task NotifyStepActivatedAsync(IReadOnlyCollection<Guid> targetUserIds, IReadOnlyCollection<object> payloads, CancellationToken cancellationToken = default);
	}
}
