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

			builder.Property(bt => bt.Amount).HasColumnType("decimal(18, 2)").IsRequired();
			builder.Property(bt => bt.Type).HasConversion<int>().IsRequired();
			builder.Property(bt => bt.TransactionDate).IsRequired();

			builder.HasOne(bt => bt.BudgetPlan)
				.WithMany(bp => bp.Transactions)
				.HasForeignKey(bt => bt.BudgetPlanId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasIndex(bt => new { bt.BudgetPlanId, bt.Type });
		}
	}
}
