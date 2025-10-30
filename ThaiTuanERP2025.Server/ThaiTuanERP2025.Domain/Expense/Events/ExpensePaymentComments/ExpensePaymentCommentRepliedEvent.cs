using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ExpensePaymentComments
{
	public sealed class ExpensePaymentCommentRepliedEvent : IDomainEvent
	{
		public ExpensePaymentCommentRepliedEvent(ExpensePaymentComment parent, ExpensePaymentComment reply)
		{
			Parent = parent;
			Reply = reply;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpensePaymentComment Parent { get; }
		public ExpensePaymentComment Reply { get; }
		public DateTime OccurredOn { get; }
	}
}
