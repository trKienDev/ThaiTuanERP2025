using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Common.Entities;
using ThaiTuanERP2025.Domain.Expense.Events.ExpensePaymentCommentAttachments;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpensePaymentCommentAttachment : AuditableEntity
	{
		private ExpensePaymentCommentAttachment() { } // EF Core

		public ExpensePaymentCommentAttachment(
			Guid commentId,
			string fileName,
			string fileUrl,
			long fileSize,
			string? mimeType,
			Guid? fileId,
			Guid createdByUserId
		) {
			Guard.AgainstDefault(commentId, nameof(commentId));
			Guard.AgainstNullOrWhiteSpace(fileName, nameof(fileName));
			Guard.AgainstNullOrWhiteSpace(fileUrl, nameof(fileUrl));
			Guard.AgainstZeroOrNegative(fileSize, nameof(fileSize));
			Guard.AgainstDefault(createdByUserId, nameof(createdByUserId));

			Id = Guid.NewGuid();
			CommentId = commentId;
			FileName = fileName.Trim();
			FileUrl = fileUrl.Trim();
			FileSize = fileSize;
			MimeType = string.IsNullOrWhiteSpace(mimeType) ? "application/octet-stream" : mimeType.Trim();
			FileId = fileId;

			AddDomainEvent(new ExpensePaymentCommentAttachmentAddedEvent(this));
		}

		public Guid CommentId { get; private set; }
		public ExpensePaymentComment Comment { get; private set; } = default!;

		public Guid? FileId { get; private set; }
		public string FileName { get; private set; } = string.Empty;
		public string FileUrl { get; private set; } = string.Empty;
		public string? MimeType { get; private set; }
		public long FileSize { get; private set; }

		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }
	}
}
