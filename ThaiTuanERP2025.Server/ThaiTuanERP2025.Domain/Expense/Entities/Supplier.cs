using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Expense.Events.Suppliers;
using ThaiTuanERP2025.Domain.Shared.Interfaces;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class Supplier : AuditableEntity, IActiveEntity
	{
		#region Constructors
		private Supplier() { }
		public Supplier(string name, string? taxCode)
		{
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Id = Guid.NewGuid();
			Name = name.Trim();
			TaxCode = string.IsNullOrWhiteSpace(taxCode) ? null : taxCode.Trim();
			IsActive = true;

			AddDomainEvent(new SupplierCreatedEvent(this));
		}
		#endregion

		#region Properties
		public string Name { get; private set; } = null!;
		public string? TaxCode { get; private set; }
		public bool IsActive { get; private set; } = true;
		#endregion

		#region Domain Behaviors
		public void Rename(string name)
		{
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			if (Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase)) return;

			Name = name.Trim();
			AddDomainEvent(new SupplierRenamedEvent(this));
		}

		public void SetTaxCode(string? taxCode)
		{
			TaxCode = string.IsNullOrWhiteSpace(taxCode) ? null : taxCode.Trim();
			AddDomainEvent(new SupplierUpdatedEvent(this));
		}

		public void Activate()
		{
			if (!IsActive)
			{
				IsActive = true;
				AddDomainEvent(new SupplierActivatedEvent(this));
			}
		}

		public void Deactivate()
		{
			if (IsActive)
			{
				IsActive = false;
				AddDomainEvent(new SupplierDeactivatedEvent(this));
			}
		}
		#endregion
	}
}
