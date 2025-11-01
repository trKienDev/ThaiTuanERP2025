using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Infrastructure.Finance.Configurations
{
	public class BudgetTransactionConfiguration : IEntityTypeConfiguration<BudgetTransaction>
	{
		public void Configure(EntityTypeBuilder<BudgetTransaction> builder)
		{
			builder.ToTable("BudgetTransaction", "Finance");

			builder.HasKey(bt => bt.Id);

			// ===== Basic properties =====
			builder.Property(x => x.Amount).HasColumnType("decimal(18,2)").IsRequired();
			builder.Property(x => x.Type).HasConversion<int>().IsRequired();
			builder.Property(x => x.TransactionDate).IsRequired();

			// ===== Indexes =====
			builder.HasIndex(x => x.TransactionDate);
			builder.HasIndex(x => x.Type);
		}
	}
}
