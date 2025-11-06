using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Infrastructure.Account.Configurations
{
	public class DepartmentManagerConfiguration : IEntityTypeConfiguration<DepartmentManager> {
		public  void Configure(EntityTypeBuilder<DepartmentManager> builder)
		{
			builder.ToTable("DepartmentManagers", "Account");

			builder.HasKey(x => x.Id);

			builder.HasIndex(x => new { x.DepartmentId, x.UserId }).IsUnique();

			// Mỗi Department chỉ có tối đa 1 primary manager (tùy nhu cầu)
			builder.HasIndex(x => new { x.DepartmentId, x.IsPrimary })
			       .HasFilter("[IsPrimary] = 1")
			       .IsUnique();

			builder.HasOne(x => x.Department)
			       .WithMany(d => d.Managers)
			       .HasForeignKey(x => x.DepartmentId)
			       .OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.User)
			       .WithMany() // không cần nav ngược ở User
			       .HasForeignKey(x => x.UserId)
			       .OnDelete(DeleteBehavior.Cascade);
		}

	}
}
