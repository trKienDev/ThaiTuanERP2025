using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public class OutgoingPaymentConfiguration : IEntityTypeConfiguration<OutgoingPayment>
	{
		public void Configure(EntityTypeBuilder<OutgoingPayment> builder)
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
			builder.Property(x => x.PostingDate).IsRequired();
			builder.Property(x => x.PaymentDate).IsRequired();

			// === Relationships ===
			builder.HasOne(o => o.ExpensePayment)
				.WithMany(e => e.OutgoingPayments)
				.HasForeignKey(o => o.ExpensePaymentId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(o => o.OutgoingBankAccount)
				.WithMany()
				.HasForeignKey(o => o.OutgoingBankAccountId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(o => o.CreatedByUser)
				.WithMany()
				.HasForeignKey("CreatedByUserId")
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(o => o.ModifiedByUser)
				.WithMany()
				.HasForeignKey("ModifiedByUserId")
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(o => o.DeletedByUser)
				.WithMany()
				.HasForeignKey("DeletedByUserId")
				.OnDelete(DeleteBehavior.Restrict);

			// === Indexes ===
			builder.HasIndex(o => o.ExpensePaymentId);
			builder.HasIndex(o => o.OutgoingBankAccountId);
			builder.HasIndex(o => o.PostingDate);
			builder.HasIndex(o => o.PaymentDate);
		}
	}
}
