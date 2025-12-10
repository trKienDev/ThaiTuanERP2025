using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Account.Configurations
{
	public class PermissionConfiguration : BaseEntityConfiguration<Permission>
	{
		public override void Configure(EntityTypeBuilder<Permission> builder)
		{
			builder.ToTable("Permissions", "RBAC");

			builder.HasKey(p => p.Id);

			builder.Property(p => p.Name).IsRequired().HasMaxLength(150);
			builder.Property(p => p.Code).IsRequired().HasMaxLength(100);
			builder.Property(p => p.Description).HasMaxLength(500);
			builder.Property(p => p.IsActive).HasDefaultValue(true);

			// Backing field for RolePermissions
			builder.Metadata.FindNavigation(nameof(Permission.RolePermissions))!
				.SetPropertyAccessMode(PropertyAccessMode.Field);

			// Unique Code
			builder.HasIndex(p => p.Code).IsUnique();

			ConfigureAuditUsers(builder);
		}
	}
}