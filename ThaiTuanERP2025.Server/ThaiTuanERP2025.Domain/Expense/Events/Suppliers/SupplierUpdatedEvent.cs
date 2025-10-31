using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.Suppliers
{
	public sealed class SupplierUpdatedEvent : IDomainEvent
	{
		public SupplierUpdatedEvent(Supplier supplier)
		{
			Supplier = supplier ?? throw new ArgumentNullException(nameof(supplier));
			OccurredOn = DateTime.UtcNow;
		}

		public Supplier Supplier { get; }
		public DateTime OccurredOn { get; }
	}
}
