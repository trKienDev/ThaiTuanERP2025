using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public class ExpensePaymentConfiguration : IEntityTypeConfiguration<ExpensePayment>
	{
		public void Configure(EntityTypeBuilder<ExpensePayment> builder) {
			builder.ToTable("ExpensePayments", "Expense");
			builder.HasKey(x => x.Id);

			// Text fields
			builder.Property(x => x.Name).HasMaxLength(256).IsRequired();
			builder.Property(x => x.BankName).HasMaxLength(128);
			builder.Property(x => x.AccountNumber).HasMaxLength(64);
			builder.Property(x => x.BeneficiaryName).HasMaxLength(128);

			// Totals precision
			builder.Property(x => x.TotalAmount).HasPrecision(18, 2);
			builder.Property(x => x.TotalTax).HasPrecision(18, 2);
			builder.Property(x => x.TotalWithTax).HasPrecision(18, 2);

			builder.Property(x => x.Status).HasConversion<int>();
			builder.Property(x => x.PayeeType).HasConversion<int>();

			// Relationships
			builder.HasMany(x => x.Items)
				.WithOne(i => i.ExpensePayment)
				.HasForeignKey(i => i.ExpensePaymentId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Attachments)
				.WithOne(a => a.ExpensePayment)
				.HasForeignKey(a => a.ExpensePaymentId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Supplier)
				.WithMany() 
				.HasForeignKey(x => x.SupplierId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasMany(x => x.Followers)
				.WithOne(f => f.ExpensePayment)
				.HasForeignKey(f => f.ExpensePaymentId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(p => p.CurrentWorkflowInstance)
				.WithMany()
				.HasForeignKey(p => p.CurrentWorkflowInstanceId)
				.OnDelete(DeleteBehavior.SetNull);
			builder.HasIndex(p => p.CurrentWorkflowInstanceId);

			// Indexes
			builder.HasIndex(x => x.SupplierId);
			builder.HasIndex(x => x.PaymentDate);
			builder.HasIndex(x => x.Status);
		}
	}
}
