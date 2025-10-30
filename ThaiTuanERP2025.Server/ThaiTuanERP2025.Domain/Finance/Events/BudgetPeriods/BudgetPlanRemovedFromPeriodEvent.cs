using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Events.BudgetPeriods
{
	public sealed class BudgetPlanRemovedFromPeriodEvent : IDomainEvent
	{
		public BudgetPlanRemovedFromPeriodEvent(BudgetPeriod budgetPeriod, BudgetPlan budgetPlan)
		{
			BudgetPeriod = budgetPeriod;
			BudgetPlan = budgetPlan;
			OccurredOn = DateTime.UtcNow;
		}

		public BudgetPeriod BudgetPeriod { get; }
		public BudgetPlan BudgetPlan { get; }
		public DateTime OccurredOn { get; }
	}
}
