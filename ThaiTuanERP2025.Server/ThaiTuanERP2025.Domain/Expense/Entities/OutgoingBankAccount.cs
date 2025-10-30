using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class OutgoingBankAccount : AuditableEntity
	{
		private OutgoingBankAccount() { } // EF only

		public OutgoingBankAccount(string name, string bankName, string accountNumber, string ownerName)
		{
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Guard.AgainstNullOrWhiteSpace(bankName, nameof(bankName));
			Guard.AgainstNullOrWhiteSpace(accountNumber, nameof(accountNumber));
			Guard.AgainstNullOrWhiteSpace(ownerName, nameof(ownerName));

			Id = Guid.NewGuid();
			Name = name.Trim();
			BankName = bankName.Trim();
			AccountNumber = accountNumber.Trim();
			OwnerName = ownerName.Trim();
			IsActive = true;

			AddDomainEvent(new OutgoingBankAccountCreatedEvent(this));
		}

		#region Properties
		public string Name { get; private set; } = null!;
		public string BankName { get; private set; } = null!;
		public string AccountNumber { get; private set; } = null!;
		public string OwnerName { get; private set; } = null!;
		public bool IsActive { get; private set; } = true;

		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }
		#endregion

		#region Domain Behaviors
		public void SetName(string name)
		{
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Name = name.Trim();
			AddDomainEvent(new OutgoingBankAccountUpdatedEvent(this));
		}

		public void UpdateBankInfo(string bankName, string accountNumber, string ownerName)
		{
			Guard.AgainstNullOrWhiteSpace(bankName, nameof(bankName));
			Guard.AgainstNullOrWhiteSpace(accountNumber, nameof(accountNumber));
			Guard.AgainstNullOrWhiteSpace(ownerName, nameof(ownerName));

			BankName = bankName.Trim();
			AccountNumber = accountNumber.Trim();
			OwnerName = ownerName.Trim();

			AddDomainEvent(new OutgoingBankAccountUpdatedEvent(this));
		}

		public void Activate()
		{
			if (!IsActive)
			{
				IsActive = true;
				AddDomainEvent(new OutgoingBankAccountActivatedEvent(this));
			}
		}

		public void Deactivate()
		{
			if (IsActive)
			{
				IsActive = false;
				AddDomainEvent(new OutgoingBankAccountDeactivatedEvent(this));
			}
		}
		#endregion
	}
}
