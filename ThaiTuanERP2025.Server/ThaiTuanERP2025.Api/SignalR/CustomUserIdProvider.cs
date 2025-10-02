using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace ThaiTuanERP2025.Api.SignalR;

public sealed class CustomUserIdProvider : IUserIdProvider
{
	public string? GetUserId(HubConnectionContext connection)
	{
		// Trả về GUID user id dạng string để dùng Clients.User(...)
		// Thường là ClaimTypes.NameIdentifier hoặc sub (OpenID)
		return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
	}
}
