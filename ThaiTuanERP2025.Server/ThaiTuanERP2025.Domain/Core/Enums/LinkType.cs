namespace ThaiTuanERP2025.Domain.Core.Enums
{
	public enum LinkType
	{
		None = 0,

		// [1] Finance
		// [1][2] BudgetPlan
		BudgetPlanReview = 111,
		BudgetPlanDetail = 112,
		BudgetPlanApproved = 113,

		// [2] Expense 
		// [2][1]: ExpensePayment
		ExpensePaymentApprove = 211,


		// [3] Request
		RequestDetail = 31,

		// Others
		Dashboard
	}
}
