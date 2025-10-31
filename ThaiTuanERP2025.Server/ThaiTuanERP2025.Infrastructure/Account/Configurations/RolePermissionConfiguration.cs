using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Infrastructure.Account.Configurations
{
	public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
	{
		public void Configure(EntityTypeBuilder<RolePermission> builder)
		{
			builder.ToTable("RolePermissions", "RBAC");

			// Composite key
			builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

			// Relationships
			builder.HasOne<Role>()
				.WithMany(r => r.RolePermissions)
				.HasForeignKey(rp => rp.RoleId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne<Permission>()
				.WithMany(p => p.RolePermissions)
				.HasForeignKey(rp => rp.PermissionId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}