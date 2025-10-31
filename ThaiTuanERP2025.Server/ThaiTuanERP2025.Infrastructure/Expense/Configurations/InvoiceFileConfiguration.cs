using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	//public class InvoiceFileConfiguration : IEntityTypeConfiguration<InvoiceFile>
	//{
	//	public void Configure(EntityTypeBuilder<InvoiceFile> builder)
	//	{
	//		builder.ToTable("InvoiceFiles", "Expense");
	//		builder.HasKey(f => f.Id);

	//		builder.Property(x => x.IsMain).IsRequired();
	//		builder.Property(x => x.CreatedAt).IsRequired();

	//		// Foreign keys
	//		builder.HasOne(x => x.Invoice)
	//			.WithMany("_files")
	//			.HasForeignKey(x => x.InvoiceId)
	//			.OnDelete(DeleteBehavior.Cascade);

	//		builder.HasOne(x => x.File)
	//			.WithMany()
	//			.HasForeignKey(x => x.FileId)
	//			.OnDelete(DeleteBehavior.Restrict);

	//		// Index: 1 hóa đơn chỉ có thể có 1 file chính
	//		builder.HasIndex(x => new { x.InvoiceId, x.IsMain }).HasFilter("[IsMain] = 1").IsUnique();

	//		// Index để truy xuất nhanh
	//		builder.HasIndex(x => x.CreatedAt);
	//	}
	//}
}
