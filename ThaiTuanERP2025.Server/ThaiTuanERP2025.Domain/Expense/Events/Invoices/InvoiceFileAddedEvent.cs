using ThaiTuanERP2025.Domain.Shared.Events;

namespace ThaiTuanERP2025.Domain.Expense.Events.Invoices
{
	public sealed class InvoiceFileAddedEvent : IDomainEvent
	{
		public InvoiceFileAddedEvent(Guid invoiceId, string fileName)
		{
			InvoiceId = invoiceId;
			FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
			OccurredOn = DateTime.UtcNow;
		}

		public Guid InvoiceId { get; }
		public string FileName { get; }
		public DateTime OccurredOn { get; }
	}
}
