using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ThaiTuanERP2025.Application.Common.Interfaces;

namespace ThaiTuanERP2025.Infrastructure.Authentication
{
	public class CurrentUserService : ICurrentUserService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		public CurrentUserService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public ClaimsPrincipal? Principal => _httpContextAccessor.HttpContext?.User;

		public Guid? UserId
		{
			get
			{
				var sub = Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value
					?? Principal?.FindFirst("sub")?.Value;

				return Guid.TryParse(sub, out var id) ? id : null;
			}
		}

		public bool IsInRole(string role) => Principal?.IsInRole(role) ?? false;

		public Guid GetUserIdOrThrow() => UserId ?? throw new UnauthorizedAccessException("user chưa được phân quyền");
	}
}
