using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ExpensePayments
{
	public sealed class ExpensePaymentRejectedEvent : IDomainEvent
	{
		public ExpensePaymentRejectedEvent(ExpensePayment expensePayment, string reason)
		{
			ExpensePayment = expensePayment;
			Reason = reason;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpensePayment ExpensePayment { get; }
		public string Reason { get; }
		public DateTime OccurredOn { get; }
	}
}
