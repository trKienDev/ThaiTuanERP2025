using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Infrastructure.Account.Configurations
{
	public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
	{
		public void Configure(EntityTypeBuilder<Permission> builder)
		{
			builder.ToTable("Permissions", "RBAC");

			builder.HasKey(p => p.Id);

			builder.Property(p => p.Name).IsRequired().HasMaxLength(150);

			builder.Property(p => p.Code).IsRequired().HasMaxLength(100);

			builder.Property(p => p.Description).HasMaxLength(500);

			builder.Property(p => p.IsActive).HasDefaultValue(true);

			builder.HasOne(e => e.CreatedByUser)
				.WithMany()
				.HasForeignKey(e => e.CreatedByUserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.ModifiedByUser)
				.WithMany()
				.HasForeignKey(e => e.ModifiedByUserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.DeletedByUser)
				.WithMany()
				.HasForeignKey(e => e.DeletedByUserId)
				.OnDelete(DeleteBehavior.Restrict);

			// Backing field for RolePermissions
			builder.Metadata.FindNavigation(nameof(Permission.RolePermissions))!
				.SetPropertyAccessMode(PropertyAccessMode.Field);

			// Unique Code
			builder.HasIndex(p => p.Code).IsUnique();
		}
	}
}
