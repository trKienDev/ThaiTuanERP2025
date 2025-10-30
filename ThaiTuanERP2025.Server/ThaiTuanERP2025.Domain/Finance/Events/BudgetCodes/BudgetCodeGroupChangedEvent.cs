using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Events.BudgetCodes
{
	public sealed class BudgetCodeGroupChangedEvent : IDomainEvent
	{
		public BudgetCodeGroupChangedEvent(BudgetCode budgetCode)
		{
			BudgetCode = budgetCode;
			OccurredOn = DateTime.UtcNow;
		}

		public BudgetCode BudgetCode { get; }
		public DateTime OccurredOn { get; }
	}
}
