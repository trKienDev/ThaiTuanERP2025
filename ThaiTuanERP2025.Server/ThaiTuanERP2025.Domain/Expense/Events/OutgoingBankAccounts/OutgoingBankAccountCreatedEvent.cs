using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.OutgoingBankAccounts
{
	public sealed class OutgoingBankAccountCreatedEvent : IDomainEvent
	{
		public OutgoingBankAccountCreatedEvent(OutgoingBankAccount account)
		{
			Account = account ?? throw new ArgumentNullException(nameof(account));
			OccurredOn = DateTime.UtcNow;
		}

		public OutgoingBankAccount Account { get; }
		public DateTime OccurredOn { get; }
	}
}
