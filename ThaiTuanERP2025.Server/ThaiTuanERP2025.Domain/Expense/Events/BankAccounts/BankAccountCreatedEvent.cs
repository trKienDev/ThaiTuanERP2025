using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.BankAccounts
{
	public sealed class BankAccountCreatedEvent : IDomainEvent
	{
		public BankAccountCreatedEvent(BankAccount bankAccount)
		{
			BankAccount = bankAccount;
			OccurredOn = DateTime.UtcNow;
		}

		public BankAccount BankAccount { get; }
		public DateTime OccurredOn { get; }
	}
}
