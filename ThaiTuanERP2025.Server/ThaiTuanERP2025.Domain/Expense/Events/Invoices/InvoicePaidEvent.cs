using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.Invoices
{
	public sealed class InvoicePaidEvent : IDomainEvent
	{
		public InvoicePaidEvent(Invoice invoice)
		{
			Invoice = invoice ?? throw new ArgumentNullException(nameof(invoice));
			OccurredOn = DateTime.UtcNow;
		}

		public Invoice Invoice { get; }
		public DateTime OccurredOn { get; }
	}
}
