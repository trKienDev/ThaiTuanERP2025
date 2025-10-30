using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ExpensePaymentComments
{
	public sealed class ExpensePaymentCommentEditedEvent : IDomainEvent
	{
		public ExpensePaymentCommentEditedEvent(ExpensePaymentComment comment)
		{
			Comment = comment;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpensePaymentComment Comment { get; }
		public DateTime OccurredOn { get; }
	}
}
