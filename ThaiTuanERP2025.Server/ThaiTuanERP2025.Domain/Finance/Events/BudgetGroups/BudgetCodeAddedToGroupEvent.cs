using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Events.BudgetGroups
{
	public sealed class BudgetCodeAddedToGroupEvent : IDomainEvent
	{
		public BudgetCodeAddedToGroupEvent(BudgetGroup budgetGroup, BudgetCode budgetCode)
		{
			BudgetGroup = budgetGroup;
			BudgetCode = budgetCode;
			OccurredOn = DateTime.UtcNow;
		}

		public BudgetGroup BudgetGroup { get; }
		public BudgetCode BudgetCode { get; }
		public DateTime OccurredOn { get; }
	}
}
