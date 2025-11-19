using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Finance.Events;
using ThaiTuanERP2025.Domain.Shared.Interfaces;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetCode : AuditableEntity, IActiveEntity
	{
		#region Constructors
		private BudgetCode() { }
		public BudgetCode(string code, string name, Guid budgetGroupId)
		{
			Guard.AgainstNullOrWhiteSpace(code, nameof(code));
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Guard.AgainstDefault(budgetGroupId, nameof(budgetGroupId));

			Id = Guid.NewGuid();
			Code = code.Trim().ToUpperInvariant();
			Name = name.Trim();
			BudgetGroupId = budgetGroupId;
			IsActive = true;

			AddDomainEvent(new BudgetCodeCreatedEvent(this));
		}
		#endregion

		#region Properties
		public string Code { get; private set; } = null!;
		public string Name { get; private set; } = null!;
		public Guid BudgetGroupId { get; private set; }
		public Guid? CashoutCodeId { get; private set; }
		public bool IsActive { get; private set; } = true;

		public BudgetGroup BudgetGroup { get; private set; } = null!;
		public CashoutCode? CashoutCode { get; private set; } = null!;
		#endregion

		#region Domain Behaviors
		internal void Rename(string newName)
		{
			Guard.AgainstNullOrWhiteSpace(newName, nameof(newName));
			Name = newName.Trim();
			AddDomainEvent(new BudgetCodeRenamedEvent(this));
		}

		internal void ChangeCode(string newCode)
		{
			Guard.AgainstNullOrWhiteSpace(newCode, nameof(newCode));
			Code = newCode.Trim().ToUpperInvariant();
			AddDomainEvent(new BudgetCodeCodeChangedEvent(this));
		}

		internal void ChangeGroup(Guid newGroupId)
		{
			Guard.AgainstDefault(newGroupId, nameof(newGroupId));
			BudgetGroupId = newGroupId;
			AddDomainEvent(new BudgetCodeGroupChangedEvent(this));
		}

		internal void Activate()
		{
			if (IsActive) return;
			IsActive = true;
			AddDomainEvent(new BudgetCodeActivatedEvent(this));
		}

		internal void Deactivate()
		{
			if (!IsActive) return;
			IsActive = false;
			AddDomainEvent(new BudgetCodeDeactivatedEvent(this));
		}

		internal void SetCashoutCode(Guid cashoutCodeId)
		{
			Guard.AgainstDefault(cashoutCodeId, nameof(cashoutCodeId));
			if (CashoutCodeId.HasValue && CashoutCodeId.Value == cashoutCodeId) return;

			var old = CashoutCodeId;
			CashoutCodeId = cashoutCodeId;
			AddDomainEvent(new BudgetCodeCashoutChangedEvent(this, old, CashoutCodeId));
		}

		internal void ClearCashoutCode()
		{
			if (CashoutCodeId is null) return;
			var old = CashoutCodeId;
			CashoutCodeId = null;
			AddDomainEvent(new BudgetCodeCashoutChangedEvent(this, old, CashoutCodeId));
		}
		#endregion
	}
}
