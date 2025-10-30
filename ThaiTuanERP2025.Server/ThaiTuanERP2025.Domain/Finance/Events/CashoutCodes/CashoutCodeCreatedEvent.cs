using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Events.CashoutCodes
{
	public sealed class CashoutCodeCreatedEvent : IDomainEvent
	{
		public CashoutCodeCreatedEvent(CashoutCode cashoutCode)
		{
			CashoutCode = cashoutCode;
			OccurredOn = DateTime.UtcNow;
		}

		public CashoutCode CashoutCode { get; }
		public DateTime OccurredOn { get; }
	}
}
