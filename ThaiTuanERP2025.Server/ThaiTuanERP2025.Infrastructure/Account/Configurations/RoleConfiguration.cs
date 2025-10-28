using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Infrastructure.Account.Configurations
{
	public class RoleConfiguration : IEntityTypeConfiguration<Role>
	{
		public void Configure(EntityTypeBuilder<Role> builder)
		{
			builder.ToTable("Roles", "RBAC");
			builder.HasKey(r => r.Id);

			builder.Property(r => r.Name).IsRequired().HasMaxLength(100);
			builder.Property(r => r.Description).HasMaxLength(255);

			builder.HasMany(r => r.UserRoles)
				.WithOne(ur => ur.Role)
				.HasForeignKey(ur => ur.RoleId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(r => r.RolePermissions)
			    .WithOne(rp => rp.Role)
			    .HasForeignKey(rp => rp.RoleId)
			    .OnDelete(DeleteBehavior.Restrict);
		}
	}
}
