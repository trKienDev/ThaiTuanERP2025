using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Finance.Events.BudgetGroups;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetGroup : AuditableEntity
	{
		private BudgetGroup() { }

		public BudgetGroup(string code, string name)
		{
			Guard.AgainstNullOrWhiteSpace(code, nameof(code));
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));

			Id = Guid.NewGuid();
			Code = code.Trim().ToUpperInvariant();
			Name = name.Trim();

			AddDomainEvent(new BudgetGroupCreatedEvent(this));
		}

		public string Code { get; private set; } = string.Empty;
		public string Name { get; private set; } = string.Empty;

		public ICollection<BudgetCode> BudgetCodes { get; private set; } = new List<BudgetCode>();

		public User CreatedByUser { get; private set; } = null!;
		public User? ModifiedByUser { get; private set; }
		public User? DeletedByUser { get; private set; }

		public void Update(string code, string name)
		{
			Guard.AgainstNullOrWhiteSpace(code, nameof(code));
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));

			Code = code.Trim().ToUpperInvariant();
			Name = name.Trim();

			AddDomainEvent(new BudgetGroupUpdatedEvent(this));
		}

		public void AddBudgetCodes(IEnumerable<Guid> budgetCodeIds, IEnumerable<BudgetCode> allBudgetCodes)
		{
			if (budgetCodeIds == null || !budgetCodeIds.Any()) return;

			var toAdd = allBudgetCodes
				.Where(b => budgetCodeIds.Contains(b.Id) && b.BudgetGroupId == Id)
				.ToList();

			foreach (var code in toAdd)
			{
				if (!BudgetCodes.Any(b => b.Id == code.Id))
				{
					BudgetCodes.Add(code);
					AddDomainEvent(new BudgetCodeAddedToGroupEvent(this, code));
				}
			}
		}

		public void RemoveBudgetCodes(IEnumerable<Guid> budgetCodeIds)
		{
			if (budgetCodeIds == null || !budgetCodeIds.Any()) return;

			var toRemove = BudgetCodes
				.Where(b => budgetCodeIds.Contains(b.Id))
				.ToList();

			foreach (var code in toRemove)
			{
				BudgetCodes.Remove(code);
				AddDomainEvent(new BudgetCodeRemovedFromGroupEvent(this, code));
			}
		}
	}
}
