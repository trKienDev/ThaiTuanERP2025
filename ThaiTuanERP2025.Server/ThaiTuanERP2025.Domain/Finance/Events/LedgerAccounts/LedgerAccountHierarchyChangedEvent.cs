using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Events.LedgerAccounts
{
	public sealed class LedgerAccountHierarchyChangedEvent : IDomainEvent
	{
		public LedgerAccountHierarchyChangedEvent(LedgerAccount ledgerAccount)
		{
			LedgerAccount = ledgerAccount;
			OccurredOn = DateTime.UtcNow;
		}

		public LedgerAccount LedgerAccount { get; }
		public DateTime OccurredOn { get; }
	}
}
