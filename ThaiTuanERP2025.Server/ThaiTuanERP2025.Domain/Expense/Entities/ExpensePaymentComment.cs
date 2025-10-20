using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpensePaymentComment : AuditableEntity
	{
		private readonly List<ExpensePaymentCommentAttachment> _attachments = new();
		private readonly List<ExpensePaymentCommentTag> _tags = new();
		private readonly List<ExpensePaymentComment> _replies = new();

		private ExpensePaymentComment() { }
		public ExpensePaymentComment(Guid paymentId, string content, Guid createdByUserId, Guid? parentCommentId = null)
		{
			Id = Guid.NewGuid();
			ExpensePaymentId = paymentId;
			Content = content.Trim();
			CreatedByUserId = createdByUserId;
			ParentCommentId = parentCommentId;
			CommentType = ExpensePaymentCommentType.Normal;
			IsEdited = false;
		}

		// Liên kết
		public Guid ExpensePaymentId { get; private set; }
		public ExpensePayment ExpensePayment { get; private set; } = default!;

		public Guid? ParentCommentId { get; private set; }
		public ExpensePaymentComment? ParentComment { get; private set; }

		public IReadOnlyCollection<ExpensePaymentComment> Replies => _replies;

		// Nội dung
		public string Content { get; private set; } = string.Empty;
		public bool IsEdited { get; private set; }
		public ExpensePaymentCommentType CommentType { get; private set; }

		// Điều hướng
		public IReadOnlyCollection<ExpensePaymentCommentAttachment> Attachments => _attachments;
		public IReadOnlyCollection<ExpensePaymentCommentTag> Tags => _tags;

		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }

		// Hành vi
		public void Edit(string newContent, Guid modifiedByUserId)
		{
			Content = newContent.Trim();
			IsEdited = true;
			ModifiedByUserId = modifiedByUserId;
			DateModified = DateTime.UtcNow;
		}

		public void AddAttachment(string fileName, string fileUrl, long fileSize, string? mimeType, Guid? fileId, Guid createdByUserId)
		{
			_attachments.Add(new ExpensePaymentCommentAttachment(Id, fileName, fileUrl, fileSize, mimeType, fileId, createdByUserId));
		}

		public void AddTag(Guid userId, Guid createdByUserId)
		{
			if (_tags.Any(t => t.UserId == userId)) return;
			_tags.Add(new ExpensePaymentCommentTag(Id, userId, createdByUserId));
		}

		public void AddReply(ExpensePaymentComment reply)
		{
			_replies.Add(reply);
		}

	}
}
