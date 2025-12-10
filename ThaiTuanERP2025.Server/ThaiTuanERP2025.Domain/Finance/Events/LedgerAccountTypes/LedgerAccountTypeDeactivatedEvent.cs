using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Events.LedgerAccountTypes
{
	public sealed class LedgerAccountTypeDeactivatedEvent : IDomainEvent
	{
		public LedgerAccountTypeDeactivatedEvent(LedgerAccountType ledgerAccountType)
		{
			LedgerAccountType = ledgerAccountType;
		}

		public LedgerAccountType LedgerAccountType { get; }
		public DateTime OccurredOn { get; }
	}
}
