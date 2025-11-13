using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Events.ExpensePaymentComments;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpensePaymentCommentTag : AuditableEntity
	{
		private ExpensePaymentCommentTag() { } // EF Core

		public ExpensePaymentCommentTag(Guid commentId, Guid userId, Guid createdByUserId)
		{
			Guard.AgainstDefault(commentId, nameof(commentId));
			Guard.AgainstDefault(userId, nameof(userId));
			Guard.AgainstDefault(createdByUserId, nameof(createdByUserId));

			if (userId == createdByUserId)
				throw new DomainException("Không thể tag chính mình trong bình luận.");

			Id = Guid.NewGuid();
			CommentId = commentId;
			UserId = userId;

			AddDomainEvent(new ExpensePaymentCommentTagAddedEvent(commentId, userId));
		}

		public Guid CommentId { get; private set; }
		public ExpensePaymentComment Comment { get; private set; } = default!;

		public Guid UserId { get; private set; }
		public User User { get; private set; } = default!;
	}
}
