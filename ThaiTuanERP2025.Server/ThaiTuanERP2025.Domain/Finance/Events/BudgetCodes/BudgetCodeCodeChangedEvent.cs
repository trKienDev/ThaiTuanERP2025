using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Events.BudgetCodes
{
	public sealed class BudgetCodeCodeChangedEvent : IDomainEvent
	{
		public BudgetCodeCodeChangedEvent(BudgetCode budgetCode)
		{
			BudgetCode = budgetCode;
			OccurredOn = DateTime.UtcNow;
		}

		public BudgetCode BudgetCode { get; }
		public DateTime OccurredOn { get; }
	}
}
