using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using ThaiTuanERP2025.Application.Shared.Interfaces;

namespace ThaiTuanERP2025.Infrastructure.Persistence
{
	public class ThaiTuanERP2025DbContextFactory : IDesignTimeDbContextFactory<ThaiTuanERP2025DbContext>
	{
		public ThaiTuanERP2025DbContext CreateDbContext(string[] args)
		{
			// 1️ ) Load appsettings.json để lấy connection string
			var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../ThaiTuanERP2025.Api");
			var config = new ConfigurationBuilder()
				.SetBasePath(basePath)
				.AddJsonFile("appsettings.json", optional: true)
				.AddJsonFile(Path.Combine(basePath, "ThaiTuanERP2025.Api", "appsettings.json"), optional: true)
				.AddEnvironmentVariables()
				.Build();

			var connectionString = config.GetConnectionString("DefaultConnection")
			    ?? config.GetConnectionString("ThaiTuanERP2025Db")
			    ?? throw new InvalidOperationException("Không tìm thấy connection string hợp lệ trong appsettings.json.");

			Console.WriteLine($"[DesignTimeFactory] BasePath: {basePath}");
			Console.WriteLine($"[DesignTimeFactory] ConnStr: {connectionString}");

			// 2️ ) Cấu hình DbContextOptions
			var optionsBuilder = new DbContextOptionsBuilder<ThaiTuanERP2025DbContext>();
			optionsBuilder.UseSqlServer(connectionString);

			// 3️ ) Tạo DbContext với mock service (design-time)
			var fakeCurrentUser = new FakeCurrentUserService();

			try
			{
				return new ThaiTuanERP2025DbContext(optionsBuilder.Options, fakeCurrentUser, null);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi khi tạo DbContext:");
				Console.WriteLine(ex);
				throw;
			}
		}
	}

	/// <summary>
	/// Mock cho ICurrentUserService trong môi trường design-time (EF migrations)
	/// </summary>
	internal class FakeCurrentUserService : ICurrentUserService
	{
		public Guid? UserId => Guid.Empty;
		public ClaimsPrincipal? Principal => null;

		public bool IsInRole(string role) => false;

		public bool hasPermission(string permission) => false;

		public Guid GetUserIdOrThrow() => Guid.Empty;
	}
}
