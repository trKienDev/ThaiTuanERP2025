using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public class ExpensePaymentItemConfiguration : IEntityTypeConfiguration<ExpensePaymentItem>
	{
		public void Configure(EntityTypeBuilder<ExpensePaymentItem> builder) {
			builder.ToTable("ExpensePaymentItems", "Expense");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.ItemName).HasMaxLength(256).IsRequired();
			builder.Property(x => x.Quantity).IsRequired();
			builder.Property(x => x.UnitPrice) .HasPrecision(18, 2).IsRequired();
			builder.Property(x => x.TaxRate).HasPrecision(5, 4).IsRequired();
			builder.Property(x => x.Amount).HasPrecision(18, 2);
			builder.Property(x => x.TaxAmount).HasPrecision(18, 2);
			builder.Property(x => x.TotalWithTax).HasPrecision(18, 2);

			builder.HasOne(i => i.Invoice)
				.WithMany() // hoặc .WithMany(inv => inv.ExpensePaymentItems)
				.HasForeignKey(i => i.InvoiceId)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(i => i.BudgetCode)
				.WithMany()
				.HasForeignKey(i => i.BudgetCodeId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(i => i.CashoutCode)
				.WithMany()
				.HasForeignKey(i => i.CashoutCodeId)
				.OnDelete(DeleteBehavior.Restrict);

			// Indexes
			builder.HasIndex(i => i.ExpensePaymentId);
			builder.HasIndex(i => i.InvoiceId);

			// Check constraints (EF Core 8+ cách mới)
			builder.ToTable(t =>
			{
				t.HasCheckConstraint("CK_ExpensePaymentItem_Quantity_Positive", "[Quantity] >= 1");
				t.HasCheckConstraint("CK_ExpensePaymentItem_UnitPrice_NonNegative", "[UnitPrice] >= 0");
				t.HasCheckConstraint("CK_ExpensePaymentItem_TaxRate_Range", "[TaxRate] >= 0 AND [TaxRate] <= 1");
			});
		}
	}
}
