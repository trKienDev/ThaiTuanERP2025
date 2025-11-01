using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Finance.Configurations
{
	public class LedgerAccountTypeConfiguration : BaseEntityConfiguration<LedgerAccountType>
	{
		public override void Configure(EntityTypeBuilder<LedgerAccountType> builder)
		{
			builder.ToTable("LedgerAccountTypes", "Finance");

			builder.HasKey(x => x.Id);

			builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
			builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
			builder.Property(x => x.Description).HasMaxLength(500);
			builder.Property(x => x.IsActive).HasDefaultValue(true);
			builder.Property(x => x.LedgerAccountTypeKind).HasConversion<int>().IsRequired();

			builder.HasIndex(x => x.Code).IsUnique();
			builder.HasIndex(x => x.IsActive);

			// Auditable
			ConfigureAuditUsers(builder);
		}
	}
}
