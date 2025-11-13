using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Events.BudgetGroups
{
	public sealed class BudgetGroupUpdatedEvent : IDomainEvent
	{
		public BudgetGroupUpdatedEvent(BudgetGroup budgetGroup)
		{
			BudgetGroup = budgetGroup;
			OccurredOn = DateTime.UtcNow;
		}

		public BudgetGroup BudgetGroup { get; }
		public DateTime OccurredOn { get; }
	}
}
