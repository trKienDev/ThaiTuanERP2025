using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ExpensePayments
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
		private Guid ApproverUserId { get; }
		private DateTime DueAt { get; }
		public ExpensePaymentCreatedEvent(ExpensePayment payment, Guid approverUserId, DateTime dueAt) : base(payment)
		{
			ApproverUserId = approverUserId;
			DueAt = dueAt;
		}
	}
}
