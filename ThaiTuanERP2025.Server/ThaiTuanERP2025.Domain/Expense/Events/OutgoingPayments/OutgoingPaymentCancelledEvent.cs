using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.OutgoingPayments
{
	public sealed class OutgoingPaymentCancelledEvent : IDomainEvent
	{
		public OutgoingPaymentCancelledEvent(OutgoingPayment payment, Guid cancelledBy)
		{
			Payment = payment ?? throw new ArgumentNullException(nameof(payment));
			CancelledBy = cancelledBy;
			OccurredOn = DateTime.UtcNow;
		}

		public OutgoingPayment Payment { get; }
		public Guid CancelledBy { get; }
		public DateTime OccurredOn { get; }
	}
}
