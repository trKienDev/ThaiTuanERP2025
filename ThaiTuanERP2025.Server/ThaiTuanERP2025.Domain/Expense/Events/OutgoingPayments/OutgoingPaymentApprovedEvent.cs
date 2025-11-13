using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.OutgoingPayments
{
	public sealed class OutgoingPaymentApprovedEvent : IDomainEvent
	{
		public OutgoingPaymentApprovedEvent(OutgoingPayment payment, Guid approvedBy)
		{
			Payment = payment ?? throw new ArgumentNullException(nameof(payment));
			ApprovedBy = approvedBy;
			OccurredOn = DateTime.UtcNow;
		}

		public OutgoingPayment Payment { get; }
		public Guid ApprovedBy { get; }
		public DateTime OccurredOn { get; }
	}
}
