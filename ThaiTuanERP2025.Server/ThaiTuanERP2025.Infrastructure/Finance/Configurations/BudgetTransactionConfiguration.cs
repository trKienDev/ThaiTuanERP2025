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

			builder.HasKey(x => x.Id);

			// ===== Basic properties =====
			builder.Property(x => x.Amount).HasColumnType("decimal(18,2)").IsRequired();
			builder.Property(x => x.Type).HasConversion<int>().IsRequired();
			builder.Property(x => x.TransactionDate).IsRequired();

			// Foreign Key
			builder.HasOne(x => x.BudgetPlanDetail)
			       .WithMany() // hoặc .WithMany(bd => bd.Transactions)
			       .HasForeignKey(x => x.BudgetPlanDetailId)
			       .OnDelete(DeleteBehavior.Restrict);

			// FK: ExpensePaymentItem
			builder.HasOne(x => x.ExpensePaymentItem)
			       .WithMany()
			       .HasForeignKey(x => x.ExpensePaymentItemId)
			       .OnDelete(DeleteBehavior.Restrict);

                        // Self reference: OriginalTransaction → ReversedTransaction
                        builder.HasOne(x => x.OriginalTransaction)
                               .WithOne(x => x.ReversedByTransaction)
                               .HasForeignKey<BudgetTransaction>(x => x.OriginalTransactionId)
                               .OnDelete(DeleteBehavior.NoAction); // tránh cascade

                        builder.HasOne(x => x.ReversedByTransaction)
                               .WithOne(x => x.OriginalTransaction)
                               .HasForeignKey<BudgetTransaction>(x => x.ReversedByTransactionId)
                               .OnDelete(DeleteBehavior.NoAction); // tránh cascade

                        // ===== Indexes =====
                        builder.HasIndex(x => x.TransactionDate);
			builder.HasIndex(x => x.Type);
                        builder.HasIndex(x => x.OriginalTransactionId);
                        builder.HasIndex(x => x.ReversedByTransactionId);
                }
	}
}
