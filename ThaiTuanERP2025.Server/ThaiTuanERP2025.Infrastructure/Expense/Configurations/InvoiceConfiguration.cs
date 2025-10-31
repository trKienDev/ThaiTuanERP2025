using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	//public class InvoiceConfiguration : BaseEntityConfiguration<Invoice>
	//{
	//	public override void Configure(EntityTypeBuilder<Invoice> builder) {
	//		builder.ToTable("Invoices", "Expense");
	//		builder.HasKey(x => x.Id);

	//		builder.Property(x => x.InvoiceNumber).HasMaxLength(50).IsRequired();
	//		builder.Property(x => x.InvoiceName).HasMaxLength(200).IsRequired();
	//		builder.Property(x => x.SellerTaxCode).HasMaxLength(50).IsRequired();
	//		builder.Property(x => x.SellerName).HasMaxLength(200);
	//		builder.Property(x => x.SellerAddress).HasMaxLength(300);
	//		builder.Property(x => x.BuyerName).HasMaxLength(200);
	//		builder.Property(x => x.BuyerTaxCode).HasMaxLength(50);
	//		builder.Property(x => x.BuyerAddress).HasMaxLength(300);

	//		// Money columns
	//		builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");
	//		builder.Property(x => x.TotalTax).HasColumnType("decimal(18,2)");
	//		builder.Property(x => x.TotalWithTax).HasColumnType("decimal(18,2)");

	//		// Flags and dates
	//		builder.Property(x => x.IsDraft).IsRequired();
	//		builder.Property(x => x.IssueDate).IsRequired();
	//		builder.Property(x => x.PaymentDate).IsRequired(false);

	//		// Private field for files
	//		builder.Navigation(nameof(Invoice.Files)).UsePropertyAccessMode(PropertyAccessMode.Field);

	//		builder.HasMany(typeof(InvoiceFile), "_files")
	//			.WithOne(nameof(InvoiceFile.Invoice))
	//			.HasForeignKey(nameof(InvoiceFile.InvoiceId))
	//			.OnDelete(DeleteBehavior.Cascade);

	//		// Index
	//		builder.HasIndex(x => x.InvoiceNumber)
	//		    .IsUnique();

	//		builder.HasIndex(x => x.IssueDate);
	//	}
	//}
}
