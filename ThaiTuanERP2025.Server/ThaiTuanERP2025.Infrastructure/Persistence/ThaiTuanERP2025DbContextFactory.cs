using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;

namespace ThaiTuanERP2025.Infrastructure.Persistence
{
	public class ThaiTuanERP2025DbContextFactory : IDesignTimeDbContextFactory<ThaiTuanERP2025DbContext> {
		public ThaiTuanERP2025DbContext CreateDbContext(string[] args) {
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json").Build();

			var connectionString = config.GetConnectionString("ThaiTuanERP2025Db");
			var optionsBuilder = new DbContextOptionsBuilder<ThaiTuanERP2025DbContext>();
			optionsBuilder.UseSqlServer(connectionString);

			return new ThaiTuanERP2025DbContext(optionsBuilder.Options, null!);
		}
	}
}
