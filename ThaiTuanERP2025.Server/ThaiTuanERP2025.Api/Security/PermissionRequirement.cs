using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ThaiTuanERP2025.Api.Security
{
	/// <summary>
	/// Requirement đại diện cho một Permission cụ thể.
	/// </summary>
	public class PermissionRequirement : IAuthorizationRequirement
	{
		public string PermissionCode { get; }

		public PermissionRequirement(string permissionCode)
		{
			PermissionCode = permissionCode;
		}
	}

	/// <summary>
	/// Handler kiểm tra logic quyền truy cập.
	/// </summary>
	public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
		{
			// Lấy danh sách role từ token (claim "role")
			var roles = context.User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

			// Nếu là SuperAdmin hoặc Admin => full quyền
			if (roles.Contains("SuperAdmin", StringComparer.OrdinalIgnoreCase) || roles.Contains("Admin", StringComparer.OrdinalIgnoreCase))
			{
				context.Succeed(requirement);
				return Task.CompletedTask;
			}

			// Nếu có claim permission tương ứng => pass
			if (context.User.HasClaim("permission", requirement.PermissionCode))
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}
}
