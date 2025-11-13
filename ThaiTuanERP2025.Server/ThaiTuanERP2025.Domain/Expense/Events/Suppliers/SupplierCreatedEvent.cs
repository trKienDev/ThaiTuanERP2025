using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.Suppliers
{
	public sealed class SupplierCreatedEvent : IDomainEvent
	{
		public SupplierCreatedEvent(Supplier supplier)
		{
			Supplier = supplier ?? throw new ArgumentNullException(nameof(supplier));
			OccurredOn = DateTime.UtcNow;
		}

		public Supplier Supplier { get; }
		public DateTime OccurredOn { get; }
	}
}
