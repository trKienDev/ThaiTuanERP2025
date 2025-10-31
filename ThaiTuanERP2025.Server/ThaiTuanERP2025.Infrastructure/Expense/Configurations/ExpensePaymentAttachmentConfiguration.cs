using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	//public class ExpensePaymentAttachmentConfiguration : IEntityTypeConfiguration<ExpensePaymentAttachment>
	//{
	//	public void Configure(EntityTypeBuilder<ExpensePaymentAttachment> builder) {
	//		builder.ToTable("ExpensePaymentAttachments", "Expense");
	//		builder.HasKey(x => x.Id);

	//		builder.Property(x => x.ObjectKey).HasMaxLength(512).IsRequired();
	//		builder.Property(x => x.FileName).HasMaxLength(256).IsRequired();
	//		builder.Property(x => x.Url).HasMaxLength(1024);
	//		builder.Property(x => x.Size).IsRequired();

	//		builder.HasIndex(x => x.ExpensePaymentId);
	//		builder.HasIndex(x => x.FileId);
	//	}
	//}
}
