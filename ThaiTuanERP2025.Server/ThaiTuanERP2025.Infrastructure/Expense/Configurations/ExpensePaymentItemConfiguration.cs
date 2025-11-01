﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public class ExpensePaymentItemConfiguration : IEntityTypeConfiguration<ExpensePaymentItem>
	{
		public void Configure(EntityTypeBuilder<ExpensePaymentItem> builder)
		{
			builder.ToTable("ExpensePaymentItems", "Expense");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.ItemName).HasMaxLength(300).IsRequired();
			builder.Property(x => x.Quantity).IsRequired();
			builder.Property(x => x.UnitPrice).HasPrecision(18, 2).IsRequired();
			builder.Property(x => x.TaxRate).HasPrecision(5, 4).IsRequired();
			builder.Property(x => x.Amount).HasPrecision(18, 2).IsRequired();
			builder.Property(x => x.TaxAmount).HasPrecision(18, 2).IsRequired();
			builder.Property(x => x.TotalWithTax).HasPrecision(18, 2).IsRequired();

			// ===== Relationships =====

			// ExpensePayment (Aggregate Root)
			builder.HasOne(x => x.ExpensePayment)
				.WithMany(p => p.Items)
				.HasForeignKey(x => x.ExpensePaymentId)
				.OnDelete(DeleteBehavior.Cascade); // chỉ cascade từ root → item

			// BudgetCode
			builder.HasOne(x => x.BudgetCode)
				.WithMany()
				.HasForeignKey(x => x.BudgetCodeId)
				.OnDelete(DeleteBehavior.Restrict);

			// CashoutCode
			builder.HasOne(x => x.CashoutCode)
				.WithMany()
				.HasForeignKey(x => x.CashoutCodeId)
				.OnDelete(DeleteBehavior.Restrict);

			// Invoice
			builder.HasOne(x => x.Invoice)
				.WithMany()
				.HasForeignKey(x => x.InvoiceId)
				.OnDelete(DeleteBehavior.Restrict);

			// ===== Indexes =====
			builder.HasIndex(x => x.ExpensePaymentId);
			builder.HasIndex(x => x.ItemName);
		}
	}
}
