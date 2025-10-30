using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Finance.Entities.ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Events.CashoutGroups
{
	public sealed class CashoutGroupCreatedEvent : IDomainEvent
	{
		public CashoutGroupCreatedEvent(CashoutGroup cashoutGroup)
		{
			CashoutGroup = cashoutGroup;
			OccurredOn = DateTime.UtcNow;
		}

		public CashoutGroup CashoutGroup { get; }
		public DateTime OccurredOn { get; }
	}
}
