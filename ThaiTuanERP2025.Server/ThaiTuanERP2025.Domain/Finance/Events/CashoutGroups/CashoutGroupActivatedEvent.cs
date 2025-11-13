using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Finance.Entities;
namespace ThaiTuanERP2025.Domain.Finance.Events.CashoutGroups
{
	public sealed class CashoutGroupActivatedEvent : IDomainEvent
	{
		public CashoutGroupActivatedEvent(CashoutGroup cashoutGroup)
		{
			CashoutGroup = cashoutGroup;
			OccurredOn = DateTime.UtcNow;
		}

		public CashoutGroup CashoutGroup { get; }
		public DateTime OccurredOn { get; }
	}
}
