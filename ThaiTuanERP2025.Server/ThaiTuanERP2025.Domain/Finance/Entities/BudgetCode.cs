using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Finance.Events.BudgetCodes;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetCode : AuditableEntity
	{
		private BudgetCode() { } // EF only

		public BudgetCode(string code, string name, Guid budgetGroupId, Guid cashoutCodeId)
		{
			Guard.AgainstNullOrWhiteSpace(code, nameof(code));
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Guard.AgainstDefault(budgetGroupId, nameof(budgetGroupId));
			Guard.AgainstDefault(cashoutCodeId, nameof(cashoutCodeId));

			Id = Guid.NewGuid();
			Code = code.Trim().ToUpperInvariant();
			Name = name.Trim();
			BudgetGroupId = budgetGroupId;
			CashoutCodeId = cashoutCodeId;
			IsActive = true;

			AddDomainEvent(new BudgetCodeCreatedEvent(this));
		}

		public string Code { get; private set; } = null!;
		public string Name { get; private set; } = null!;
		public Guid BudgetGroupId { get; private set; }
		public Guid CashoutCodeId { get; private set; }
		public bool IsActive { get; private set; } = true;

		public BudgetGroup BudgetGroup { get; private set; } = null!;
		public CashoutCode CashoutCode { get; private set; } = null!;
		public ICollection<BudgetPlan> BudgetPlans { get; private set; } = new List<BudgetPlan>();

		public User CreatedByUser { get; private set; } = null!;
		public User? ModifiedByUser { get; private set; }
		public User? DeletedByUser { get; private set; }

		#region Domain Behaviors

		public void Rename(string newName)
		{
			Guard.AgainstNullOrWhiteSpace(newName, nameof(newName));
			Name = newName.Trim();
			AddDomainEvent(new BudgetCodeRenamedEvent(this));
		}

		public void ChangeCode(string newCode)
		{
			Guard.AgainstNullOrWhiteSpace(newCode, nameof(newCode));
			Code = newCode.Trim().ToUpperInvariant();
			AddDomainEvent(new BudgetCodeCodeChangedEvent(this));
		}

		public void ChangeGroup(Guid newGroupId)
		{
			Guard.AgainstDefault(newGroupId, nameof(newGroupId));
			BudgetGroupId = newGroupId;
			AddDomainEvent(new BudgetCodeGroupChangedEvent(this));
		}

		public void Activate()
		{
			if (IsActive) return;
			IsActive = true;
			AddDomainEvent(new BudgetCodeActivatedEvent(this));
		}

		public void Deactivate()
		{
			if (!IsActive) return;
			IsActive = false;
			AddDomainEvent(new BudgetCodeDeactivatedEvent(this));
		}

		#endregion
	}
}
