using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Events.BudgetTransactions
{
	public sealed class BudgetTransactionAdjustedEvent : IDomainEvent
	{
		public BudgetTransactionAdjustedEvent(BudgetTransaction budgetTransaction)
		{
			BudgetTransaction = budgetTransaction;
			OccurredOn = DateTime.UtcNow;
		}

		public BudgetTransaction BudgetTransaction { get; }
		public DateTime OccurredOn { get; }
	}
}
