using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.OutgoingBankAccounts
{
	public sealed class OutgoingBankAccountUpdatedEvent : IDomainEvent
	{
		public OutgoingBankAccountUpdatedEvent(OutgoingBankAccount account)
		{
			Account = account ?? throw new ArgumentNullException(nameof(account));
			OccurredOn = DateTime.UtcNow;
		}

		public OutgoingBankAccount Account { get; }
		public DateTime OccurredOn { get; }
	}
}
