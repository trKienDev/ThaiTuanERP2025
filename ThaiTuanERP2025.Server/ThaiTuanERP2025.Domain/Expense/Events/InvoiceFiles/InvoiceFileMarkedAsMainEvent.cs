using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.InvoiceFiles
{

	public sealed class InvoiceFileMarkedAsMainEvent : IDomainEvent
	{
		public InvoiceFileMarkedAsMainEvent(InvoiceFile file)
		{
			File = file;
			OccurredOn = DateTime.UtcNow;
		}

		public InvoiceFile File { get; }
		public DateTime OccurredOn { get; }
	}
}
