using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ExpensePaymentItems
{
	public sealed class ExpensePaymentItemLinkedInvoiceEvent : IDomainEvent
	{
		public ExpensePaymentItemLinkedInvoiceEvent(ExpensePaymentItem item, Guid invoiceId)
		{
			Item = item;
			InvoiceId = invoiceId;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpensePaymentItem Item { get; }
		public Guid InvoiceId { get; }
		public DateTime OccurredOn { get; }
	}
}
