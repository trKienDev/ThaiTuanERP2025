using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.OutgoingPayments
{
	public sealed class OutgoingPaymentCreatedEvent : IDomainEvent
	{
		public OutgoingPaymentCreatedEvent(OutgoingPayment payment)
		{
			Payment = payment ?? throw new ArgumentNullException(nameof(payment));
			OccurredOn = DateTime.UtcNow;
		}

		public OutgoingPayment Payment { get; }
		public DateTime OccurredOn { get; }
	}
}
