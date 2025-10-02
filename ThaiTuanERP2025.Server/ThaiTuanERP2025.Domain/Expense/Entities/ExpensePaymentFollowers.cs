using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpensePaymentFollower : AuditableEntity
	{
		private ExpensePaymentFollower() { } // EF

		public ExpensePaymentFollower(Guid expensePaymentId, Guid userId)
		{
			Id = Guid.NewGuid();
			ExpensePaymentId = expensePaymentId;
			UserId = userId;
		}

		public Guid ExpensePaymentId { get; private set; }
		public ExpensePayment ExpensePayment { get; private set; } = null!;

		public Guid UserId { get; private set; }
		public User User { get; private set; } = null!;
	}
}
