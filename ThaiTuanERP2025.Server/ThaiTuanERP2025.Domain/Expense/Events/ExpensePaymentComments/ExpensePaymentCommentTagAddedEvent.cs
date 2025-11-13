using ThaiTuanERP2025.Domain.Shared.Events;
namespace ThaiTuanERP2025.Domain.Expense.Events.ExpensePaymentComments
{
	public sealed class ExpensePaymentCommentTagAddedEvent : IDomainEvent
	{
		public ExpensePaymentCommentTagAddedEvent(Guid commentId, Guid taggedUserId)
		{
			CommentId = commentId;
			TaggedUserId = taggedUserId;
			OccurredOn = DateTime.UtcNow;
		}

		public Guid CommentId { get; }
		public Guid TaggedUserId { get; }
		public DateTime OccurredOn { get; }
	}
}
