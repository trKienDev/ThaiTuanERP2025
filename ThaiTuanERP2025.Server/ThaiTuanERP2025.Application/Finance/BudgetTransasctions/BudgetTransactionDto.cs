using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Application.Finance.BudgetTransasctions
{
	public sealed record BudgetTransactionDto
	{
		public Guid BudgetPlanDetailId { get; init; }
		public BudgetPlanDetail BudgetPlanDetail { get; init; } = null!;

		public Guid ExpensePaymentItemId { get; init; }
		public ExpensePaymentItem ExpensePaymentItem { get; init; } = null!;

		public decimal Amount { get; init; }
		public BudgetTransactionType Type { get; init; }
		public DateTime TransactionDate { get; init; }
	}
}
