using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public class ExpensePaymentAttachmentConfiguration : BaseEntityConfiguration<ExpensePaymentAttachment>
	{
		public override void Configure(EntityTypeBuilder<ExpensePaymentAttachment> builder)
		{
			builder.ToTable("ExpensePaymentAttachments", "Expense");
			builder.HasKey(x => x.Id);

			// Relationships
			builder.HasOne(x => x.ExpensePayment)
				.WithMany(p => p.Attachments)
				.HasForeignKey(x => x.ExpensePaymentId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.StoredFile)
				.WithMany() // StoredFile không cần navigation ngược
				.HasForeignKey(x => x.StoredFileId)
				.OnDelete(DeleteBehavior.Restrict);

			// Indexes
			builder.HasIndex(x => x.ExpensePaymentId);
			builder.HasIndex(x => x.StoredFileId);

			// Audit
			ConfigureAuditUsers(builder);
		}
	}
}
