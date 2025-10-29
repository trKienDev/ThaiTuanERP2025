using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	/// <summary>
	/// Ghi nhận bút toán thay đổi ngân sách (ledger entry) cho từng BudgetPlan.
	/// </summary>
	public class BudgetTransaction : AuditableEntity
	{
		private BudgetTransaction() { }

		public BudgetTransaction(Guid budgetPlanId, Guid paymentId, decimal amount, BudgetTransactionType type)
		{
			if (amount <= 0)
				throw new ArgumentException("Số tiền phải lớn hơn 0.", nameof(amount));

			BudgetPlanId = budgetPlanId;
			PaymentId = paymentId;
			Amount = amount;
			Type = type;
			TransactionDate = DateTime.UtcNow;
		}

		/// Foreign key - BudgetPlan.
		public Guid BudgetPlanId { get; private set; }

		/// Id của thực thể nghiệp vụ gây ra giao dịch (ExpensePaymentId, RefundId...).
		public Guid PaymentId { get; private set; }

		/// Số tiền thay đổi ngân sách.
		public decimal Amount { get; private set; }

		/// Loại giao dịch ngân sách: Expense, Refund, Reserve, Adjust...
		public BudgetTransactionType Type { get; private set; }

		/// Ngày ghi nhận giao dịch.
		public DateTime TransactionDate { get; private set; }

		// --- Navigation ---
		public BudgetPlan BudgetPlan { get; private set; } = null!;
	}
}
