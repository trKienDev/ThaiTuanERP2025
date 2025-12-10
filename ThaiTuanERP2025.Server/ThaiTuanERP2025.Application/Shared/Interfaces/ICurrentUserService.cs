using System.Security.Claims;

namespace ThaiTuanERP2025.Application.Shared.Interfaces
{
	public interface ICurrentUserService
	{
		Guid? UserId { get; }
		ClaimsPrincipal? Principal { get; }

		bool IsInRole(string role);
		bool hasPermission(string permission) => false;

		Guid GetUserIdOrThrow();					
	}
}
