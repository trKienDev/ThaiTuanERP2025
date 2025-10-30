using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ExpensePaymentComments
{
	public sealed class ExpensePaymentCommentAddedEvent : IDomainEvent
	{
		public ExpensePaymentCommentAddedEvent(ExpensePaymentComment comment)
		{
			Comment = comment;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpensePaymentComment Comment { get; }
		public DateTime OccurredOn { get; }
	}
}
