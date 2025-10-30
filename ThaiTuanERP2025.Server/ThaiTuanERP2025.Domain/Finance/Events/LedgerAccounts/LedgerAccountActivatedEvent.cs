using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Events.LedgerAccounts
{

	public sealed class LedgerAccountActivatedEvent : IDomainEvent
	{
		public LedgerAccountActivatedEvent(LedgerAccount ledgerAccount)
		{
			LedgerAccount = ledgerAccount;
			OccurredOn = DateTime.UtcNow;
		}

		public LedgerAccount LedgerAccount { get; }
		public DateTime OccurredOn { get; }
	}
}
