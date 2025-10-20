using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpensePaymentCommentAttachment : AuditableEntity
	{
		private ExpensePaymentCommentAttachment() { }

		public ExpensePaymentCommentAttachment(Guid commentId, string fileName, string fileUrl, long fileSize, string? mimeType, Guid? fileId, Guid createdByUserId)
		{
			Id = Guid.NewGuid();
			CommentId = commentId;
			FileName = fileName;
			FileUrl = fileUrl;
			FileSize = fileSize;
			MimeType = mimeType ?? "application/octet-stream";
			FileId = fileId;
			CreatedByUserId = createdByUserId;
			CreatedDate = DateTime.UtcNow;
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
