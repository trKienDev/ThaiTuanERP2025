using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.StoredFiles.Entities;

namespace Drive.Infrastructure.Persistence
{
	public sealed class ThaiTuanERP2025DriveDbContext : DbContext
	{
		public ThaiTuanERP2025DriveDbContext(DbContextOptions<ThaiTuanERP2025DriveDbContext> options)
		    : base(options)
		{
		}

		public DbSet<StoredObject> StoredFiles => Set<StoredObject>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Apply all IEntityTypeConfiguration in this assembly
			modelBuilder.ApplyConfigurationsFromAssembly(
			    typeof(ThaiTuanERP2025DriveDbContext).Assembly);

			base.OnModelCreating(modelBuilder);
		}
	}
}
