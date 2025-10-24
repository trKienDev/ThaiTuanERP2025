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

			builder.Property(p => p.Code).IsRequired().HasMaxLength(150);
			builder.HasIndex(p => p.Code).IsUnique();

			builder.Property(p => p.Description).HasMaxLength(255);
		}
	}
}
