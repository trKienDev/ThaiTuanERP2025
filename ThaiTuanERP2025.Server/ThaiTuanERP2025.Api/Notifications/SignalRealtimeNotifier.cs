using Microsoft.AspNetCore.SignalR;
using ThaiTuanERP2025.Api.Hubs;
using ThaiTuanERP2025.Application.Notifications.Services;

namespace ThaiTuanERP2025.Api.Notifications
{
	public class SignalRealtimeNotifier : IRealtimeNotifier
	{
		private readonly IHubContext<NotificationsHub> _hub;
		public SignalRealtimeNotifier(IHubContext<NotificationsHub> hub)
		{
			_hub = hub;
		}

		public async Task NotifyStepActivatedAsync(IReadOnlyCollection<Guid> targetUserIds, IReadOnlyCollection<object> payloads, CancellationToken cancellationToken = default)
		{
			if (targetUserIds.Count == 0 || payloads.Count == 0) return;

			var userIds = targetUserIds.Select(x => x.ToString()).ToList();

			// "ReceiveNotification" là event client-side sẽ lắng nghe
			await _hub.Clients.Users(userIds).SendAsync("ReceiveNotification", payloads, cancellationToken);
		}

		public async Task PushAlarmsAsync(IEnumerable<Guid> userIds, IEnumerable<object> payloads, CancellationToken cancellationToken = default)
		{
			var ids = userIds.Select(x => x.ToString()).ToList();
			if (!ids.Any() || !payloads.Any()) return;

			// client Angular lắng nghe event "ReceiveAlarm"
			await _hub.Clients.Users(ids).SendAsync("ReceiveAlarm", payloads, cancellationToken);
		}

		// mới: thông báo alarm đã được resolve (approved/expired/dismissed)
		public async Task PushAlarmResolvedAsync(IEnumerable<Guid> userIds, IEnumerable<Guid> alarmIds, CancellationToken cancellationToken = default)
		{
			var ids = userIds.Select(x => x.ToString()).ToList();
			if (!ids.Any() || !alarmIds.Any()) return;

			// client Angular lắng nghe event "ResolveAlarm"
			await _hub.Clients.Users(ids).SendAsync("ResolveAlarm", alarmIds, cancellationToken);
		}
	}
}
