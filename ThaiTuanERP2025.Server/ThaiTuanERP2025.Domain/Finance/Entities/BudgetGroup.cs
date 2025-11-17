using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Finance.Events.BudgetGroups;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetGroup : AuditableEntity
	{
		#region Constructors
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
		#endregion

		#region properties
		public string Code { get; private set; } = string.Empty;
		public string Name { get; private set; } = string.Empty;

		public ICollection<BudgetCode> BudgetCodes { get; private set; } = new List<BudgetCode>();
		#endregion

		#region Domain Behaviors
		internal void Update(string code, string name)
		{
			Guard.AgainstNullOrWhiteSpace(code, nameof(code));
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));

			Code = code.Trim().ToUpperInvariant();
			Name = name.Trim();

			AddDomainEvent(new BudgetGroupUpdatedEvent(this));
		}

		internal void AddBudgetCodes(IEnumerable<Guid> budgetCodeIds, IEnumerable<BudgetCode> allBudgetCodes)
		{
			if (budgetCodeIds is null) return;

			var ids = budgetCodeIds as ISet<Guid> ?? budgetCodeIds.ToHashSet();
			if (ids.Count == 0) return;

			var existingIds = BudgetCodes.Select(b => b.Id).ToHashSet();

			var toAdd = allBudgetCodes
				.Where(b => b.BudgetGroupId == Id && ids.Contains(b.Id) && !existingIds.Contains(b.Id))
				.ToList();

			foreach (var code in toAdd)
			{
				BudgetCodes.Add(code);
				AddDomainEvent(new BudgetCodeAddedToGroupEvent(this, code));
			}
		}

		internal void RemoveBudgetCodes(IEnumerable<Guid> budgetCodeIds)
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
		#endregion
	}
}
