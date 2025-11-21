using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Finance.Configurations
{
	public class CashoutCodeConfiguration : BaseEntityConfiguration<CashoutCode>
	{
		public override void Configure(EntityTypeBuilder<CashoutCode> builder)
		{
			builder.ToTable("CashoutCodes", "Finance").HasIndex(x => x.Id);

			// ===== Basic properties =====
			builder.Property(x => x.Name).HasMaxLength(256).IsRequired();
			builder.Property(x => x.Description).HasMaxLength(1000);
			builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);

			// ===== Relationships =====
			// CashoutGroup (1-n)
			builder.HasOne(x => x.CashoutGroup)
				.WithMany(x => x.CashoutCodes)
				.HasForeignKey(x => x.CashoutGroupId)
				.OnDelete(DeleteBehavior.Cascade);

			// Posting Ledger Account (1-n)
			builder.HasOne(x => x.PostingLedgerAccount)
				.WithMany()
				.HasForeignKey(x => x.PostingLedgerAccountId)
				.OnDelete(DeleteBehavior.Restrict);

			// BudgetCodes (1-n)
			builder.HasMany(x => x.BudgetCodes)
				.WithOne(x => x.CashoutCode)
				.HasForeignKey(x => x.CashoutCodeId)
				.OnDelete(DeleteBehavior.Restrict);

			// ===== Indexes =====
			builder.HasIndex(x => new { x.CashoutGroupId }).IsUnique(); // Code duy nhất trong mỗi nhóm
			builder.HasIndex(x => x.IsActive);
			builder.HasIndex(x => x.Name).IsUnique();

			// Auditable
			ConfigureAuditUsers(builder);
		}
	}
}
