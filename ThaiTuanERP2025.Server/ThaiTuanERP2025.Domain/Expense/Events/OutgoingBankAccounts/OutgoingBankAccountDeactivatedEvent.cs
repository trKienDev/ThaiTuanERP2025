using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.OutgoingBankAccounts
{
	public sealed class OutgoingBankAccountDeactivatedEvent : IDomainEvent
	{
		public OutgoingBankAccountDeactivatedEvent(OutgoingBankAccount account)
		{
			Account = account ?? throw new ArgumentNullException(nameof(account));
			OccurredOn = DateTime.UtcNow;
		}

		public OutgoingBankAccount Account { get; }
		public DateTime OccurredOn { get; }
	}
}
