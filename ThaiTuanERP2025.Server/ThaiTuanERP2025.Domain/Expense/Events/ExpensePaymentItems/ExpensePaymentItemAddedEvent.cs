using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ExpensePaymentItems
{
	public sealed class ExpensePaymentItemAddedEvent : IDomainEvent
	{
		public ExpensePaymentItemAddedEvent(ExpensePaymentItem item)
		{
			Item = item;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpensePaymentItem Item { get; }
		public DateTime OccurredOn { get; }
	}
}
