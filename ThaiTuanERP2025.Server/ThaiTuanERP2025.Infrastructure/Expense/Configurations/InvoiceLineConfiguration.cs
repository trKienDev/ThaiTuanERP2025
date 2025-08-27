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
	public class InvoiceLineConfiguration : IEntityTypeConfiguration<InvoiceLine>
	{
		public void Configure(EntityTypeBuilder<InvoiceLine> builder)
		{
			builder.ToTable("InvoiceLines", "Expense");
			builder.HasKey(l => l.Id);

			builder.Property(l => l.ItemCode).HasMaxLength(50);
			builder.Property(l => l.ItemName).IsRequired().HasMaxLength(250);
			builder.Property(l => l.Unit).HasMaxLength(50);

			builder.Property(l => l.Quantity).HasColumnType("decimal(18, 2)");
			builder.Property(l => l.UnitPrice).HasColumnType("decimal(18, 2)");

			builder.Property(l => l.DiscountRate).HasColumnType("decimal(5, 2)");
			builder.Property(l => l.DiscountAmount).HasColumnType("decimal(18, 2)");	
			builder.Property(l => l.NetAmount).HasColumnType("decimal(18, 2)");
			builder.Property(l => l.VATAmount).HasColumnType("decimal(18, 2)");
			builder.Property(l => l.WHTAmount).HasColumnType("decimal(18, 2)");
			builder.Property(l => l.LineTotal).HasColumnType("decimal(18, 2)");

			// Relationships
			builder.HasOne(l => l.Tax)
				.WithMany()
				.HasForeignKey(l => l.TaxId)
				.OnDelete(DeleteBehavior.Restrict);	
			builder.HasOne(l => l.WHTType)
				.WithMany()
				.HasForeignKey(l => l.WHTTypeId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
