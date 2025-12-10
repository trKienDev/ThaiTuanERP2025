using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Finance.Configurations
{
	public class CashoutGroupConfiguration : BaseEntityConfiguration<CashoutGroup>
	{
		public override void Configure(EntityTypeBuilder<CashoutGroup> builder)
		{
			builder.ToTable("CashoutGroups", "Finance").HasKey(x => x.Id);

			// ===== Basic properties =====
			builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
			builder.Property(x => x.Description).HasMaxLength(500);
			builder.Property(x => x.Level).IsRequired().HasDefaultValue(0);
			builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
			builder.Property(x => x.OrderNumber).IsRequired().HasDefaultValue(0);
			builder.Property(x => x.Path).HasMaxLength(450).IsRequired();

			// ===== Self-referencing hierarchy (Parent - Children) =====
			builder.HasOne(x => x.Parent)
				.WithMany(x => x.Children)
				.HasForeignKey(x => x.ParentId)
				.OnDelete(DeleteBehavior.Restrict);

			// ===== Navigation to CashoutCodes =====
			builder.HasMany(x => x.CashoutCodes)
				.WithOne(x => x.CashoutGroup)
				.HasForeignKey(x => x.CashoutGroupId)
				.OnDelete(DeleteBehavior.Cascade);

			// ===== Indexes =====
			builder.HasIndex(x => x.IsActive);
			builder.HasIndex(x => x.Name).IsUnique();

			// Auditable
			ConfigureAuditUsers(builder);
		}
	}
}
