using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using ThaiTuanERP2025.Api.Security;

namespace ThaiTuanERP2025.Api.Startup
{
	/// <summary>
	/// Gọi trong Program.cs: await app.LoadDynamicPoliciesAsync();
	/// </summary>
	public static class AuthorizationPolicyStartupExtensions
	{
		public static async Task LoadDynamicPoliciesAsync(this WebApplication app)
		{
			using var scope = app.Services.CreateScope();

			// Lấy AuthorizationOptions qua Options pattern
			var authOptions = scope.ServiceProvider.GetRequiredService<IOptions<AuthorizationOptions>>().Value;

			// Nạp toàn bộ policy Permission:* từ DB
			await authOptions.AddPermissionPoliciesFromDatabaseAsync(scope.ServiceProvider);

			app.Logger.LogInformation("✅ Dynamic authorization policies loaded successfully.");
		}
	}
}
