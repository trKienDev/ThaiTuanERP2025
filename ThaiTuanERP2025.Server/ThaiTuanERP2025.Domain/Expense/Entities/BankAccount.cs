using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class BankAccount : AuditableEntity
	{
		private BankAccount() { } // EF only

		public BankAccount(string bankName, string accountNumber, string beneficiaryName, Guid? userId = null, Guid? supplierId = null)
		{
			Guard.AgainstNullOrWhiteSpace(bankName, nameof(bankName));
			Guard.AgainstNullOrWhiteSpace(accountNumber, nameof(accountNumber));
			Guard.AgainstNullOrWhiteSpace(beneficiaryName, nameof(beneficiaryName));

			if (userId is null && supplierId is null)
				throw new DomainException("Tài khoản ngân hàng phải thuộc về người dùng hoặc nhà cung cấp.");

			if (userId is not null && supplierId is not null)
				throw new DomainException("Tài khoản ngân hàng không thể cùng lúc thuộc về người dùng và nhà cung cấp.");

			Id = Guid.NewGuid();
			BankName = bankName.Trim();
			AccountNumber = accountNumber.Trim();
			BeneficiaryName = beneficiaryName.Trim();
			UserId = userId;
			SupplierId = supplierId;
			IsActive = true;

			AddDomainEvent(new BankAccountCreatedEvent(this));
		}

		public string BankName { get; private set; } = null!;
		public string AccountNumber { get; private set; } = null!;
		public string BeneficiaryName { get; private set; } = null!;

		public Guid? UserId { get; private set; }
		public User? User { get; private set; }

		public Guid? SupplierId { get; private set; }
		public Supplier? Supplier { get; private set; }

		public bool IsActive { get; private set; } = true;

		#region Domain Behaviors

		public void Deactivate()
		{
			if (!IsActive) return;
			IsActive = false;
			AddDomainEvent(new BankAccountDeactivatedEvent(this));
		}

		public void Activate()
		{
			if (IsActive) return;
			IsActive = true;
			AddDomainEvent(new BankAccountActivatedEvent(this));
		}

		public void ChangeBeneficiaryName(string newName)
		{
			Guard.AgainstNullOrWhiteSpace(newName, nameof(newName));
			BeneficiaryName = newName.Trim();
			AddDomainEvent(new BankAccountUpdatedEvent(this));
		}

		#endregion
	}
}
