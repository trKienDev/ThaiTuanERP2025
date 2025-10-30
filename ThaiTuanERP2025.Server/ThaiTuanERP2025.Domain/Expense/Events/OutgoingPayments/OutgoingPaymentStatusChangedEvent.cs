using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Domain.Expense.Events.OutgoingPayments
{
	public sealed class OutgoingPaymentStatusChangedEvent : IDomainEvent
	{
		public OutgoingPaymentStatusChangedEvent(OutgoingPayment payment, OutgoingPaymentStatus newStatus)
		{
			Payment = payment ?? throw new ArgumentNullException(nameof(payment));
			NewStatus = newStatus;
			OccurredOn = DateTime.UtcNow;
		}

		public OutgoingPayment Payment { get; }
		public OutgoingPaymentStatus NewStatus { get; }
		public DateTime OccurredOn { get; }
	}
}
