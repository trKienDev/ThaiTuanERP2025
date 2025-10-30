using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.Invoices
{
	public sealed class InvoiceFinalizedEvent : IDomainEvent
	{
		public InvoiceFinalizedEvent(Invoice invoice)
		{
			Invoice = invoice ?? throw new ArgumentNullException(nameof(invoice));
			OccurredOn = DateTime.UtcNow;
		}

		public Invoice Invoice { get; }
		public DateTime OccurredOn { get; }
	}
}
