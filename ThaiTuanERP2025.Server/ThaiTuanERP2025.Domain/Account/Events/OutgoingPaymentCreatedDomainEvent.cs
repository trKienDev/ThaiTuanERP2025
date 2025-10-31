using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Account.Events
{
	public sealed class OutgoingPaymentCreatedDomainEvent : IDomainEvent
	{
		public OutgoingPaymentCreatedDomainEvent(OutgoingPayment payment, Guid createdByUserId)
		{
			Payment = payment;
			CreatedByUserId = createdByUserId;
			OccurredOn = DateTime.UtcNow;
		}

		public OutgoingPayment Payment { get; }
		public Guid CreatedByUserId { get; }
		public DateTime OccurredOn { get; }
	}
}
