using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	//public class ExpensePaymentCommentConfiguration : BaseEntityConfiguration<ExpensePaymentComment>
	//{
	//	public override void Configure(EntityTypeBuilder<ExpensePaymentComment> builder) {
	//		builder.ToTable("ExpensePaymentComments", "Expense");
	//		builder.HasKey(x => x.Id);

	//		builder.Property(x => x.Content).IsRequired().HasMaxLength(2048);
	//		builder.Property(x => x.CommentType).HasConversion<int>().IsRequired();

	//		builder.HasOne(x => x.ExpensePayment)
	//			.WithMany()
	//			.HasForeignKey(x => x.ExpensePaymentId)
	//			.OnDelete(DeleteBehavior.Cascade);

	//		builder.HasOne(x => x.ParentComment)
	//			.WithMany(p => p.Replies)
	//			.HasForeignKey(x => x.ParentCommentId)
	//			.OnDelete(DeleteBehavior.Restrict);

	//		builder.HasMany(x => x.Attachments)
	//			.WithOne(a => a.Comment)
	//			.HasForeignKey(a => a.CommentId)
	//			.OnDelete(DeleteBehavior.Cascade);

	//		builder.HasMany(x => x.Tags)
	//			.WithOne(t => t.Comment)
	//			.HasForeignKey(t => t.CommentId)
	//			.OnDelete(DeleteBehavior.Cascade);

	//		builder.HasIndex(x => x.ExpensePaymentId);
	//		builder.HasIndex(x => x.ParentCommentId);
	//		builder.HasIndex(x => x.CreatedByUserId);
	//		builder.HasIndex(x => x.CreatedDate);
	//	}
	//}
}
