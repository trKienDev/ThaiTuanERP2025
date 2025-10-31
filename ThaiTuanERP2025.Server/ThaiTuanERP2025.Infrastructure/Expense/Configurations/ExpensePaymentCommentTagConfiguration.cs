using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	//public class ExpensePaymentCommentTagConfiguration : IEntityTypeConfiguration<ExpensePaymentCommentTag>
	//{
	//	public void Configure(EntityTypeBuilder<ExpensePaymentCommentTag> builder)
	//	{
	//		builder.ToTable("ExpensePaymentCommentTags", "Expense");
	//		builder.HasKey(x => x.Id);

	//		builder.HasIndex(x => x.CommentId);
	//		builder.HasIndex(x => x.UserId);

	//		builder.HasOne(x => x.User)
	//			.WithMany()
	//			.HasForeignKey(x => x.UserId)
	//			.OnDelete(DeleteBehavior.Restrict);

	//		builder.HasOne(e => e.CreatedByUser)
	//			.WithMany()
	//			.HasForeignKey(e => e.CreatedByUserId)
	//			.OnDelete(DeleteBehavior.Restrict);

	//		builder.HasOne(e => e.ModifiedByUser)
	//			.WithMany()
	//			.HasForeignKey(e => e.ModifiedByUserId)
	//			.OnDelete(DeleteBehavior.Restrict);

	//		builder.HasOne(e => e.DeletedByUser)
	//			.WithMany()
	//			.HasForeignKey(e => e.DeletedByUserId)
	//			.OnDelete(DeleteBehavior.Restrict);
	//	}
	//}
}
