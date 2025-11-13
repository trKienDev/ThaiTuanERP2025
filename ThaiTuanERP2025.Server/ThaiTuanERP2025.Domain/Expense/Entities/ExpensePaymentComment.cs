using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;
using ThaiTuanERP2025.Domain.Expense.Events.ExpensePaymentComments;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpensePaymentComment : AuditableEntity
	{
		private readonly List<ExpensePaymentCommentAttachment> _attachments = new();
		private readonly List<ExpensePaymentCommentTag> _tags = new();
		private readonly List<ExpensePaymentComment> _replies = new();

		private ExpensePaymentComment() { } // EF
		public ExpensePaymentComment(Guid expensePaymentId, string content, Guid createdByUserId, Guid? parentCommentId = null)
		{
			Guard.AgainstDefault(expensePaymentId, nameof(expensePaymentId));
			Guard.AgainstNullOrWhiteSpace(content, nameof(content));
			Guard.AgainstDefault(createdByUserId, nameof(createdByUserId));

			Id = Guid.NewGuid();
			ExpensePaymentId = expensePaymentId;
			Content = content.Trim();
			CreatedByUserId = createdByUserId;
			ParentCommentId = parentCommentId;
			CommentType = ExpensePaymentCommentType.Normal;
			IsEdited = false;

			AddDomainEvent(new ExpensePaymentCommentAddedEvent(this));
		}

		// Liên kết
		public Guid ExpensePaymentId { get; private set; }
		public ExpensePayment ExpensePayment { get; private set; } = null!;

		public Guid? ParentCommentId { get; private set; }
		public ExpensePaymentComment? ParentComment { get; private set; }

		public IReadOnlyCollection<ExpensePaymentComment> Replies => _replies.AsReadOnly();

		// Nội dung
		public string Content { get; private set; } = string.Empty;
		public bool IsEdited { get; private set; }
		public ExpensePaymentCommentType CommentType { get; private set; }


		// Phụ trợ
		public IReadOnlyCollection<ExpensePaymentCommentAttachment> Attachments => _attachments.AsReadOnly();
		public IReadOnlyCollection<ExpensePaymentCommentTag> Tags => _tags.AsReadOnly();

		#region Behaviors

		public void Edit(string newContent, Guid modifiedByUserId)
		{
			Guard.AgainstNullOrWhiteSpace(newContent, nameof(newContent));
			Guard.AgainstDefault(modifiedByUserId, nameof(modifiedByUserId));

			Content = newContent.Trim();
			IsEdited = true;

			AddDomainEvent(new ExpensePaymentCommentEditedEvent(this));
		}

		public void AddAttachment(string fileName, string fileUrl, long fileSize, string? mimeType, Guid? fileId, Guid createdByUserId)
		{
			Guard.AgainstNullOrWhiteSpace(fileName, nameof(fileName));
			Guard.AgainstNullOrWhiteSpace(fileUrl, nameof(fileUrl));
			Guard.AgainstNegativeOrZero(fileSize, nameof(fileSize));

			var attachment = new ExpensePaymentCommentAttachment(Id, fileName, fileUrl, fileSize, mimeType, fileId, createdByUserId);
			_attachments.Add(attachment);

			AddDomainEvent(new ExpensePaymentCommentAttachmentAddedEvent(this, attachment));
		}

		public void AddTag(Guid userId, Guid createdByUserId)
		{
			Guard.AgainstDefault(userId, nameof(userId));
			if (_tags.Any(t => t.UserId == userId)) return;

			var tag = new ExpensePaymentCommentTag(Id, userId, createdByUserId);
			_tags.Add(tag);

			AddDomainEvent(new ExpensePaymentCommentTagAddedEvent(this.Id, userId));
		}

		public void AddReply(ExpensePaymentComment reply)
		{
			Guard.AgainstNull(reply, nameof(reply));
			_replies.Add(reply);

			AddDomainEvent(new ExpensePaymentCommentRepliedEvent(this, reply));
		}

		#endregion
	}
}
