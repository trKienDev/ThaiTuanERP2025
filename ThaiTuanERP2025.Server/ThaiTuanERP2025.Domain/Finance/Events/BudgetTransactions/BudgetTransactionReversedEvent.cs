using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Events.BudgetTransactions
{
	public sealed class BudgetTransactionReversedEvent : IDomainEvent
	{
		public BudgetTransactionReversedEvent(BudgetTransaction budgetTransaction)
		{
			BudgetTransaction = budgetTransaction;
			OccurredOn = DateTime.UtcNow;
		}

		public BudgetTransaction BudgetTransaction { get; }
		public DateTime OccurredOn { get; }
	}
}
