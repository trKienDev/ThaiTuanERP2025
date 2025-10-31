using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Account.Configurations
{
	public class RoleConfiguration : BaseEntityConfiguration<Role>
	{
		public override void Configure(EntityTypeBuilder<Role> builder)
		{
			builder.ToTable("Roles", "RBAC");

			builder.HasKey(r => r.Id);

			builder.Property(r => r.Description).HasMaxLength(250);

			builder.OwnsOne(
				r => r.Name,
				name =>
				{
					name.Property(n => n.Value).HasColumnName("Name").IsRequired().HasMaxLength(100);
					name.HasIndex(n => n.Value).IsUnique();
				}
			);

			builder.Property(r => r.IsActive).HasDefaultValue(true);

			// UserRoles
			builder.Metadata.FindNavigation(nameof(Role.UserRoles))!.SetPropertyAccessMode(PropertyAccessMode.Field);

			// RolePermissions
			builder.Metadata.FindNavigation(nameof(Role.RolePermissions))!.SetPropertyAccessMode(PropertyAccessMode.Field);

			ConfigureAuditUsers(builder);
		}
	}
}