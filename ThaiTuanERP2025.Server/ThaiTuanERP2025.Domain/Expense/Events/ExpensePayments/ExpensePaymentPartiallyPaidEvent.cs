using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ExpensePayments
{
	public sealed class ExpensePaymentPartiallyPaidEvent : IDomainEvent
	{
		public ExpensePaymentPartiallyPaidEvent(ExpensePayment expensePayment)
		{
			ExpensePayment = expensePayment;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpensePayment ExpensePayment { get; }
		public DateTime OccurredOn { get; }
	}
}
