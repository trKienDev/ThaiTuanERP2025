using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Shared.Enums;

namespace ThaiTuanERP2025.Domain.Core.Entities
{
	public sealed class Comment : AuditableEntity
	{
		#region Constructor
		private Comment() { }
		public Comment(DocumentType documentType, Guid documentId, Guid userId, string content)
		{
			Guard.AgainstDefault(userId, nameof(userId));
			Guard.AgainstNullOrWhiteSpace(content, nameof(content));

			Id = Guid.NewGuid();
			DocumentType = documentType;
			DocumentId = documentId;
			UserId = userId;
			Content = content.Trim();
		}
		#endregion 

		#region Properties
		public DocumentType DocumentType { get; private set; }   // vd: "ExpensePayment"
		public Guid DocumentId { get; private set; }                   // Id của document
                public string Content { get; private set; } = string.Empty;

                public Guid UserId { get; private set; }
		public User User { get; set; } = default!;

                public Guid? ParentCommentId { get; private set; }
                public Comment? ParentComment { get; set; }
                public ICollection<Comment> Replies { get; private set; } = new List<Comment>();

                #endregion

                #region Domain Behaviors
                internal void UpdateContent(string content)
		{
			Guard.AgainstNullOrWhiteSpace(content, nameof(content));
			Content = content.Trim();
		}

		internal void AddReply(Comment reply)
		{
			Guard.AgainstNull(reply, nameof(reply));

			reply.ParentCommentId = this.Id;
			Replies.Add(reply);
		}

                #endregion
        }
}
