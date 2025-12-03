namespace ThaiTuanERP2025.Domain.Core.Enums
{
	public enum LinkType
	{
		None,

		// [1] Finance
		// [1][2] BudgetPlan
		BudgetPlanReview,
		BudgetPlanDetail,
		BudgetPlanApproved,


		// [2] Expense 
		// [2][1]: ExpensePayment
		ExpensePaymentDetail,

		// [3] Request
		RequestDetail,

		// Others
		Dashboard
	}
}
