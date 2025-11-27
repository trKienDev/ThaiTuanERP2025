using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Shared.Events;

namespace ThaiTuanERP2025.Domain.Expense.Events
{
	public abstract class ExpensePaymentEventBase : IDomainEvent
	{
		public ExpensePayment ExpensePayment { get; }
		public DateTime OccurredOn { get; }
		protected ExpensePaymentEventBase(ExpensePayment expensePayment)
		{
			ExpensePayment = expensePayment;
			OccurredOn = DateTime.UtcNow;
		}
	}

	public sealed class ExpensePaymentCreatedEvent : ExpensePaymentEventBase
	{
		public ExpensePaymentCreatedEvent(ExpensePayment expensePayment) : base(expensePayment) { }
	}
}
