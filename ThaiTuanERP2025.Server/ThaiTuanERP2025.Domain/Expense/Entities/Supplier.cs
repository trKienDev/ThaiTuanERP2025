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
                public string? BeneficiaryAccountNumber { get; private set; }
                public string? BeneficiaryName { get; private set; }
                public string? BeneficiaryBankName { get; private set; }
                #endregion

                #region Domain Behaviors
                internal void Rename(string name)
		{
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			if (Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase)) return;

			Name = name.Trim();
			AddDomainEvent(new SupplierRenamedEvent(this));
		}

		internal void SetTaxCode(string? taxCode)
		{
			TaxCode = string.IsNullOrWhiteSpace(taxCode) ? null : taxCode.Trim();
			AddDomainEvent(new SupplierUpdatedEvent(this));
		}

		internal void Activate()
		{
			if (!IsActive)
			{
				IsActive = true;
				AddDomainEvent(new SupplierActivatedEvent(this));
			}
		}

		internal void Deactivate()
		{
			if (IsActive)
			{
				IsActive = false;
				AddDomainEvent(new SupplierDeactivatedEvent(this));
			}
		}
                #endregion

                internal void SetBeneficiaryInfo(string? accountNumber, string? beneficiaryName, string? bankName)
                {
                        // Chuẩn hóa dữ liệu (trim hoặc null nếu empty)
                        var newAccountNumber = string.IsNullOrWhiteSpace(accountNumber) ? null : accountNumber.Trim();
                        var newBeneficiaryName = string.IsNullOrWhiteSpace(beneficiaryName) ? null : beneficiaryName.Trim();
                        var newBankName = string.IsNullOrWhiteSpace(bankName) ? null : bankName.Trim();

                        // Nếu không có thay đổi gì → không raise event
                        bool accountChanged = !string.Equals(BeneficiaryAccountNumber, newAccountNumber, StringComparison.OrdinalIgnoreCase);
                        bool nameChanged = !string.Equals(BeneficiaryName, newBeneficiaryName, StringComparison.OrdinalIgnoreCase);
                        bool bankChanged = !string.Equals(BeneficiaryBankName, newBankName, StringComparison.OrdinalIgnoreCase);

                        if (!accountChanged && !nameChanged && !bankChanged) return;

                        // Cập nhật giá trị
                        BeneficiaryAccountNumber = newAccountNumber;
                        BeneficiaryName = newBeneficiaryName;
                        BeneficiaryBankName = newBankName;
                }
        }
}
