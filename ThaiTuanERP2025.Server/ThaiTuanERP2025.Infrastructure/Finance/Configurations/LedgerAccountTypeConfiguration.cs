using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Finance.Configurations
{
	public class LedgerAccountTypeConfiguration : BaseEntityConfiguration<LedgerAccountType>
	{
		public override void Configure(EntityTypeBuilder<LedgerAccountType> builder) {
			builder.ToTable("LedgerAccountTypes", "Finance").HasKey(x => x.Id);

			builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
			builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
			builder.Property(x => x.Description).HasMaxLength(500);
			builder.Property(x => x.IsActive).IsRequired();
			builder.Property(x => x.LedgerAccountTypeKind).HasConversion<int>().IsRequired();

			// EF truy cập private field _ledgerAccounts
			builder.Navigation(nameof(LedgerAccountType.LedgerAccounts))
				.UsePropertyAccessMode(PropertyAccessMode.Field);

			builder.HasMany(typeof(LedgerAccount), "_ledgerAccounts")
				.WithOne(nameof(LedgerAccount.LedgerAccountType))
				.HasForeignKey(nameof(LedgerAccount.LedgerAccountTypeId))
				.OnDelete(DeleteBehavior.Cascade);

			// --------- Indexes ------------
			builder.HasIndex(x => x.Code).IsUnique();
			builder.HasIndex(x => x.IsActive);
		}
	}
}
