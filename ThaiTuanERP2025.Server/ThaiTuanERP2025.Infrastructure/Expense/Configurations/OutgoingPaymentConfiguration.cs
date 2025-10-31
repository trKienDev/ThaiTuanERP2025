using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public class OutgoingPaymentConfiguration : BaseEntityConfiguration<OutgoingPayment>
	{
		public override void Configure(EntityTypeBuilder<OutgoingPayment> builder)
		{
			builder.ToTable("OutgoingPayments", "Expense");
			builder.HasKey(x => x.Id);

			// === Property mapping ===
			builder.Property(x => x.Name).HasMaxLength(256).IsRequired();
			builder.Property(x => x.Description).HasMaxLength(2048);
			builder.Property(x => x.BankName).HasMaxLength(128);
			builder.Property(x => x.AccountNumber).HasMaxLength(64);
			builder.Property(x => x.BeneficiaryName).HasMaxLength(256);
			builder.Property(x => x.OutgoingAmount).HasPrecision(18, 2).IsRequired();
			builder.Property(x => x.DueDate).IsRequired();
			builder.Property(x => x.PostingDate);
			builder.Property(x => x.PaymentDate);
			builder.Property(x => x.Status)
			       .HasConversion<int>() 
			       .HasMaxLength(30)
			       .IsRequired();

			// === Relationships ===
			builder.HasOne(o => o.ExpensePayment)
				.WithMany(e => e.OutgoingPayments)
				.HasForeignKey(o => o.ExpensePaymentId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(o => o.OutgoingBankAccount)
				.WithMany()
				.HasForeignKey(o => o.OutgoingBankAccountId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(o => o.Supplier)
				.WithMany()
				.HasForeignKey("SupplierId")
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(o => o.Employee)
				.WithMany()
				.HasForeignKey("EmployeeId")
				.OnDelete(DeleteBehavior.Restrict);

			// === Indexes ===
			builder.HasIndex(o => o.ExpensePaymentId);
			builder.HasIndex(o => o.OutgoingBankAccountId);
			builder.HasIndex(o => o.DueDate);
			builder.HasIndex(o => o.PostingDate);
			builder.HasIndex(o => o.PaymentDate);
		}
	}
}
