using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class LedgerAccount : AuditableEntity
	{
		private LedgerAccount() { }

		public LedgerAccount(
			string number,
			string name,
			Guid ledgerAccountTypeId,
			LedgerAccountBalanceType balanceType,
			string? description = null,
			Guid? parentLedgerAccountId = null
		) {
			Guard.AgainstNullOrWhiteSpace(number, nameof(number));
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Guard.AgainstDefault(ledgerAccountTypeId, nameof(ledgerAccountTypeId));

			Id = Guid.NewGuid();
			Number = number.Trim().ToUpperInvariant();
			Name = name.Trim();
			LedgerAccountTypeId = ledgerAccountTypeId;
			LedgerAccountBalanceType = balanceType;
			Description = description?.Trim();
			ParentLedgerAccountId = parentLedgerAccountId;
			Path = "/";
			Level = 0;
			IsActive = true;

			AddDomainEvent(new LedgerAccountCreatedEvent(this));
		}

		public string Number { get; private set; } = null!;
		public string Name { get; private set; } = null!;
		public Guid? LedgerAccountTypeId { get; private set; }
		public Guid? ParentLedgerAccountId { get; private set; }

		public string Path { get; private set; } = "/";
		public int Level { get; private set; }
		public string? Description { get; private set; }
		public bool IsActive { get; private set; } = true;

		public LedgerAccountType LedgerAccountType { get; private set; } = null!;
		public LedgerAccount? Parent { get; private set; }
		public LedgerAccountBalanceType LedgerAccountBalanceType { get; private set; }
		public ICollection<LedgerAccount> Children { get; private set; } = new List<LedgerAccount>();
		public ICollection<CashoutCode> CashoutCodes { get; private set; } = new List<CashoutCode>();

		public User CreatedByUser { get; private set; } = null!;
		public User? ModifiedByUser { get; private set; }
		public User? DeletedByUser { get; private set; }

		#region Domain Behaviors

		public void Rename(string newName)
		{
			Guard.AgainstNullOrWhiteSpace(newName, nameof(newName));
			Name = newName.Trim();
			AddDomainEvent(new LedgerAccountRenamedEvent(this));
		}

		public void ChangeNumber(string newNumber)
		{
			Guard.AgainstNullOrWhiteSpace(newNumber, nameof(newNumber));
			Number = newNumber.Trim().ToUpperInvariant();
			AddDomainEvent(new LedgerAccountRenumberedEvent(this));
		}

		public void SetParent(Guid? parentId, int level, string path)
		{
			if (parentId == Id)
				throw new DomainException("Tài khoản không thể làm cha chính nó.");

			ParentLedgerAccountId = parentId;
			Level = level;
			Path = path;
			AddDomainEvent(new LedgerAccountHierarchyChangedEvent(this));
		}

		public void Activate()
		{
			if (IsActive) return;
			IsActive = true;
			AddDomainEvent(new LedgerAccountActivatedEvent(this));
		}

		public void Deactivate()
		{
			if (!IsActive) return;
			IsActive = false;
			AddDomainEvent(new LedgerAccountDeactivatedEvent(this));
		}

		#endregion
	}
}
