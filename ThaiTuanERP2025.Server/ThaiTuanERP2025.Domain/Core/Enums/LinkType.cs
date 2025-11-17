namespace ThaiTuanERP2025.Domain.Core.Enums
{
	public enum LinkType
	{
		None = 0,

		// [1] Expense
		// [1][1] BudgetPlan
		BudgetPlanReview = 111,
		BudgetPlanDetail = 112,
		BudgetPlanApproved = 113,

		// [1][2] Expense 
		ExpensePaymentDetail = 121,

		// [2] Request
		RequestDetail = 31,

		// Others
		Dashboard
	}
}
