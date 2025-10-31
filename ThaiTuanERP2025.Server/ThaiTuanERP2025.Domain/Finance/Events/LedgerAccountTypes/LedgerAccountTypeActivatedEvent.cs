using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Events.LedgerAccountTypes
{
	public sealed class LedgerAccountTypeActivatedEvent : IDomainEvent
	{
		public LedgerAccountTypeActivatedEvent(LedgerAccountType ledgerAccountType)
		{
			LedgerAccountType = ledgerAccountType;
		}

		public LedgerAccountType LedgerAccountType { get; }
		public DateTime OccurredOn { get; }
	}
}
