using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Api.Hubs;
using Microsoft.AspNetCore.SignalR;


namespace ThaiTuanERP2025.Infrastructure.Realtime
{
	public sealed class SignalRealtimeNotifier : IRealtimeNotifier
	{
		private readonly IHubContext<NotificationsHub> _hub;
		public SignalRealtimeNotifier(IHubContext<NotificationsHub> hub)
		{
			_hub = hub;
		}

		public async Task PushNotificationsAsync(IEnumerable<Guid> userIds, IEnumerable<object> payloads, CancellationToken cancellationToken = default)
		{
			var ids = userIds.Select(u => u.ToString()).ToArray();
			if (!ids.Any() || !payloads.Any()) return;

			await _hub.Clients.Users(ids).SendAsync("ReceiveNotification", payloads, cancellationToken);
		}

		public async Task PushRemindersAsync(IEnumerable<Guid> userIds, IEnumerable<object> payloads, CancellationToken cancellationToken = default)
		{
			var ids = userIds.Select(u => u.ToString()).ToArray();
			if (!ids.Any() || !payloads.Any()) return;

			await _hub.Clients.Users(ids).SendAsync("ReceiveReminder", payloads, cancellationToken);
		}
	}
}
