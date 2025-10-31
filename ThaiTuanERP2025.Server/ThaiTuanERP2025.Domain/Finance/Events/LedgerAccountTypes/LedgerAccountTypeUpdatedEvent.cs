﻿using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Events.LedgerAccountTypes
{
	public sealed class LedgerAccountTypeUpdatedEvent : IDomainEvent
	{
		public LedgerAccountTypeUpdatedEvent(LedgerAccountType ledgerAccountType)
		{
			LedgerAccountType = ledgerAccountType;
		}

		public LedgerAccountType LedgerAccountType { get; }
		public DateTime OccurredOn { get; }
	}
}
