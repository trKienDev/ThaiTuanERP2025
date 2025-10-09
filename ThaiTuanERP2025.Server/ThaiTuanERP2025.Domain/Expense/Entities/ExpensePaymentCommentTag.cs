using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpensePaymentCommentTag : AuditableEntity
	{
		private ExpensePaymentCommentTag() { }

		public ExpensePaymentCommentTag(Guid commentId, Guid userId, Guid createdByUserId)
		{
			Id = Guid.NewGuid();
			CommentId = commentId;
			UserId = userId;
			CreatedByUserId = createdByUserId;
			CreatedDate = DateTime.UtcNow;
		}

		public Guid CommentId { get; private set; }
		public ExpensePaymentComment Comment { get; private set; } = default!;

		public Guid UserId { get; private set; }
		public User User { get; private set; } = default!;
	}
}
