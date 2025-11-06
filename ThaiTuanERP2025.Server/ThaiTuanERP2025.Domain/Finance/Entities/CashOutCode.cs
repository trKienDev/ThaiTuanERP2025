using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Common.Entities;
using ThaiTuanERP2025.Domain.Finance.Events;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class CashoutCode : AuditableEntity
	{
		#region Constructors
		private CashoutCode() { } 
		public CashoutCode(string code, string name, Guid cashoutGroupId, Guid postingLedgerAccountId, string? description = null)
		{
			Guard.AgainstNullOrWhiteSpace(code, nameof(code));
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Guard.AgainstDefault(cashoutGroupId, nameof(cashoutGroupId));
			Guard.AgainstDefault(postingLedgerAccountId, nameof(postingLedgerAccountId));

			Id = Guid.NewGuid();
			Code = code.Trim().ToUpperInvariant();
			Name = name.Trim();
			CashoutGroupId = cashoutGroupId;
			PostingLedgerAccountId = postingLedgerAccountId;
			Description = description?.Trim();
			IsActive = true;

			AddDomainEvent(new CashoutCodeCreatedEvent(this));
		}
		#endregion

		#region Properties
		public string Code { get; private set; } = null!;
		public string Name { get; private set; } = null!;
		public Guid CashoutGroupId { get; private set; }
		public Guid PostingLedgerAccountId { get; private set; }
		public string? Description { get; private set; }
		public bool IsActive { get; private set; } = true;

		public CashoutGroup CashoutGroup { get; private set; } = null!;
		public LedgerAccount PostingLedgerAccount { get; private set; } = null!;
		public ICollection<BudgetCode> BudgetCodes { get; private set; } = new List<BudgetCode>();
		#endregion

		#region Domain Behaviors

		public void Rename(string newName)
		{
			Guard.AgainstNullOrWhiteSpace(newName, nameof(newName));
			Name = newName.Trim();
			AddDomainEvent(new CashoutCodeRenamedEvent(this));
		}

		public void ChangeLedgerAccount(Guid newLedgerAccountId)
		{
			Guard.AgainstDefault(newLedgerAccountId, nameof(newLedgerAccountId));
			PostingLedgerAccountId = newLedgerAccountId;
			AddDomainEvent(new CashoutCodeLedgerChangedEvent(this));
		}

		public void ChangeDescription(string? description)
		{
			Description = description?.Trim();
		}

		public void Activate()
		{
			if (IsActive) return;
			IsActive = true;
			AddDomainEvent(new CashoutCodeActivatedEvent(this));
		}

		public void Deactivate()
		{
			if (!IsActive) return;
			IsActive = false;
			AddDomainEvent(new CashoutCodeDeactivatedEvent(this));
		}

		#endregion
	}
}
