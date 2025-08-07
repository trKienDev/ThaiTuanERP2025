using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
		public Guid? UserId
		{
			get
			{
				var user = _httpContextAccessor.HttpContext?.User;
				var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

				return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
			}
		}
	}
}
