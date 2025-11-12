using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ThaiTuanERP2025.Api.Hubs
{
	public sealed class NotificationsHub : Hub
	{
		public override async Task OnConnectedAsync()
		{
			var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrEmpty(userId))
			{
				Console.WriteLine("[SignalR] Unauthorized connection attempt");
				Context.Abort(); // Tự chặn connect không hợp lệ
				return;
			}

			Console.WriteLine($"✅ Client connected: {userId}");
			await base.OnConnectedAsync();
		}
	}
}
