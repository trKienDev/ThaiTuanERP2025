using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
	{
		public void Configure(EntityTypeBuilder<Invoice> builder) {
			builder.ToTable("Invoices", "Expense");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.InvoiceNumber).IsRequired().HasMaxLength(50);
			builder.Property(x => x.InvoiceName).IsRequired().HasMaxLength(250);
			builder.Property(x => x.InvoiceNumber).IsRequired().HasMaxLength(50);
			builder.Property(x => x.SellerName).IsRequired().HasMaxLength(250);
			builder.Property(x => x.SellerTaxCode).IsRequired().HasMaxLength(50);
			builder.Property(x => x.SellerAddress).HasMaxLength(500);
			builder.Property(x => x.BuyerName).HasMaxLength(250);
			builder.Property(x => x.BuyerTaxCode).HasMaxLength(50);
			builder.Property(x => x.BuyerAddress).HasMaxLength(500);
			builder.Property(x => x.IsDraft).HasDefaultValue(true);
		
			// Relationship
			builder.HasMany(x => x.Lines)
				.WithOne(l => l.Invoice)
				.HasForeignKey(l => l.InvoiceId);
			builder.HasMany(x => x.Files)
				.WithOne(l => l.Invoice)
				.HasForeignKey(l => l.InvoiceId);
			builder.HasMany(x => x.Follwers)
				.WithOne(f => f.Invoice)
				.HasForeignKey(f => f.InvoiceId);
		}
	}
}
