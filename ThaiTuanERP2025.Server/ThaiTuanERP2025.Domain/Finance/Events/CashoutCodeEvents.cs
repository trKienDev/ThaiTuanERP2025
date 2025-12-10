using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Events
{
	public abstract class CashoutCodeEvents : IDomainEvent
	{
		public Guid CashoutCodeId { get; }
		public DateTime OccurredOn { get; }
		protected CashoutCodeEvents(Guid cashoutCodeId) {
			CashoutCodeId = cashoutCodeId;
			OccurredOn = DateTime.UtcNow;
		}
	}

	public sealed class CashoutCodeActivatedEvent : CashoutCodeEvents
	{
		public CashoutCodeActivatedEvent(CashoutCode cashoutCode) : base(cashoutCode.Id)
		{
			CashoutCode = cashoutCode;
		}
		public CashoutCode CashoutCode { get; }
	}

	public sealed class CashoutCodeCreatedEvent : CashoutCodeEvents
	{
		public CashoutCodeCreatedEvent(CashoutCode cashoutCode) : base (cashoutCode.Id) 
		{
			CashoutCode = cashoutCode;
		}
		public CashoutCode CashoutCode { get; }
	}

	public sealed class CashoutCodeDeactivatedEvent : CashoutCodeEvents
	{
		public CashoutCodeDeactivatedEvent(CashoutCode cashoutCode) : base(cashoutCode.Id)
		{
			CashoutCode = cashoutCode;
		}
		public CashoutCode CashoutCode { get; }
	}

	public sealed class CashoutCodeLedgerChangedEvent : CashoutCodeEvents
	{
		public CashoutCodeLedgerChangedEvent(CashoutCode cashoutCode) : base(cashoutCode.Id)
		{
			CashoutCode = cashoutCode;
		}
		public CashoutCode CashoutCode { get; }
	}

	public sealed class CashoutCodeRenamedEvent : CashoutCodeEvents
	{
		public CashoutCodeRenamedEvent(CashoutCode cashoutCode) : base(cashoutCode.Id)
		{
			CashoutCode = cashoutCode;
		}
		public CashoutCode CashoutCode { get; }
	}
}
