using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Events.Suppliers;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class Supplier : AuditableEntity
	{
		private readonly List<BankAccount> _bankAccounts = new();

		private Supplier() { } // EF

		public Supplier(string name, string? taxCode)
		{
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Id = Guid.NewGuid();
			Name = name.Trim();
			TaxCode = string.IsNullOrWhiteSpace(taxCode) ? null : taxCode.Trim();
			IsActive = true;

			AddDomainEvent(new SupplierCreatedEvent(this));
		}

		public string Name { get; private set; } = null!;
		public string? TaxCode { get; private set; }
		public bool IsActive { get; private set; } = true;

		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }

		public IReadOnlyCollection<BankAccount> BankAccounts => _bankAccounts.AsReadOnly();

		// ===== Domain behaviors =====
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

		public void AddBankAccount(BankAccount account)
		{
			if (account == null)
				throw new DomainException("Tài khoản ngân hàng không hợp lệ.");

			if (_bankAccounts.Any(b => b.AccountNumber == account.AccountNumber))
				throw new DomainException("Tài khoản ngân hàng đã tồn tại.");

			_bankAccounts.Add(account);
		}

		public void RemoveBankAccount(Guid bankAccountId)
		{
			var existing = _bankAccounts.FirstOrDefault(b => b.Id == bankAccountId);
			if (existing != null)
				_bankAccounts.Remove(existing);
		}
	}
}
