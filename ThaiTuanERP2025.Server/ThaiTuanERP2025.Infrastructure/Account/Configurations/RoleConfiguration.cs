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

			builder.Property(r => r.Description).HasMaxLength(250);

			// ===== Value Object: RoleName =====
			builder.OwnsOne(r => r.Name, name =>
			{
				name.Property(n => n.Value)
					.HasColumnName("RoleName")
					.IsRequired()
					.HasMaxLength(100);

				name.HasIndex(n => n.Value).IsUnique();
			});

			builder.Property(r => r.IsActive).HasDefaultValue(true);

			// ===== Private collections =====
			builder.Metadata.FindNavigation(nameof(Role.UserRoles))!
				.SetPropertyAccessMode(PropertyAccessMode.Field);

			builder.Metadata.FindNavigation(nameof(Role.RolePermissions))!
				.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
