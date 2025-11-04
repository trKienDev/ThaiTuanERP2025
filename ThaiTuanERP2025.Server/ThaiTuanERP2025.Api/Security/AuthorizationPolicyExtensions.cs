using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Api.Security
{
	public static class AuthorizationPolicyExtensions
	{
		public static async Task AddPermissionPoliciesFromDatabaseAsync(this AuthorizationOptions options, IServiceProvider serviceProvider)
		{
			using var scope = serviceProvider.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<ThaiTuanERP2025DbContext>();

			var permissions = await dbContext.Set<Permission>()
			    .AsNoTracking()
			    .Select(p => p.Code)
			    .ToListAsync();

			foreach (var permissionCode in permissions)
			{
				// Đăng ký policy "Permission:{code}"
				options.AddPolicy($"Permission:{permissionCode}", policy =>
				{
					policy.Requirements.Add(new PermissionRequirement(permissionCode));
				});
			}
		}
	}
}
