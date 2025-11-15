using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public class ExpensePaymentCommentAttachmentConfiguration : IEntityTypeConfiguration<ExpensePaymentCommentAttachment>
	{
		public void Configure(EntityTypeBuilder<ExpensePaymentCommentAttachment> builder)
		{
			builder.ToTable("ExpensePaymentCommentAttachments", "Expense");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.FileName).HasMaxLength(256).IsRequired();
			builder.Property(x => x.FileUrl).HasMaxLength(512).IsRequired();
			builder.Property(x => x.MimeType).HasMaxLength(128);

			builder.HasOne(e => e.CreatedByUser)
				.WithMany()
				.HasForeignKey(e => e.CreatedByUserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.ModifiedByUser)
				.WithMany()
				.HasForeignKey(e => e.ModifiedByUserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.DeletedByUser)
				.WithMany()
				.HasForeignKey(e => e.DeletedByUserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(x => x.CommentId);
		}
	}
}
