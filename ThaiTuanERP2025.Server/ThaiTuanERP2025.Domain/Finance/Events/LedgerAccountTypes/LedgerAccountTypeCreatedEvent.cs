using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Events.LedgerAccountTypes
{
	public sealed class LedgerAccountTypeCreatedEvent : IDomainEvent
	{
		public LedgerAccountTypeCreatedEvent(LedgerAccountType ledgerAccountType)
		{
			LedgerAccountType = ledgerAccountType;
		}

		public LedgerAccountType LedgerAccountType { get; }
		public DateTime OccurredOn { get; }
	}
}
