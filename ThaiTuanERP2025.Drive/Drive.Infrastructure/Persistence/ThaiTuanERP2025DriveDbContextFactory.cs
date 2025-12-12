using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Drive.Infrastructure.Persistence
{
	public sealed class ThaiTuanERP2025DriveDbContextFactory : IDesignTimeDbContextFactory<ThaiTuanERP2025DriveDbContext>
	{
		public ThaiTuanERP2025DriveDbContext CreateDbContext(string[] args)
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false)
				.AddJsonFile("appsettings.Development.json", optional: true)
				.AddEnvironmentVariables()
				.Build();

			var connectionString = configuration.GetConnectionString("ThaiTuanERP2025DriveDb");

			if (string.IsNullOrWhiteSpace(connectionString))
				throw new InvalidOperationException("Connection string 'ThaiTuanERP2025DriveDb' not found.");


			var options = new DbContextOptionsBuilder<ThaiTuanERP2025DriveDbContext>()
				.UseSqlServer(connectionString).Options;

			return new ThaiTuanERP2025DriveDbContext(options);
		}
	}
}
