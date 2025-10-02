using Microsoft.AspNetCore.SignalR;

namespace ThaiTuanERP2025.Api.Hubs
{
	public interface INotificationsClient
	{
		Task PushNewNotification(); // client tự gọi API để load list
	}

	public class NotificationsHub : Hub<INotificationsClient>
	{
	}
}
