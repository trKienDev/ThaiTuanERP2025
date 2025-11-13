using ThaiTuanERP2025.Application.Shared.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.OutgoingPayments.Events
{
	public sealed class OutgoingPaymentCreatedCommandEvent : IApplicationEvent
	{
		public OutgoingPaymentCreatedCommandEvent(OutgoingPayment payment, Guid createdByUserId)
		{
			Payment = payment ?? throw new ArgumentNullException(nameof(payment));
			CreatedByUserId = createdByUserId;
			OccurredOn = DateTime.UtcNow;
		}

		public OutgoingPayment Payment { get; }
		public Guid CreatedByUserId { get; }
		public DateTime OccurredOn { get; }
	}
}
