using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ExpensePaymentItems
{
	public sealed class ExpensePaymentItemUnlinkedInvoiceEvent : IDomainEvent
	{
		public ExpensePaymentItemUnlinkedInvoiceEvent(ExpensePaymentItem item)
		{
			Item = item;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpensePaymentItem Item { get; }
		public DateTime OccurredOn { get; }
	}
}
