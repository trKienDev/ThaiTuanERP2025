using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Events.BudgetPeriods
{
	public sealed class BudgetPeriodCreatedEvent : IDomainEvent
	{
		public BudgetPeriodCreatedEvent(BudgetPeriod budgetPeriod)
		{
			BudgetPeriod = budgetPeriod;
			OccurredOn = DateTime.UtcNow;
		}

		public BudgetPeriod BudgetPeriod { get; }
		public DateTime OccurredOn { get; }
	}
}
