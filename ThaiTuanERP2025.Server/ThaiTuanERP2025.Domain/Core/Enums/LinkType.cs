namespace ThaiTuanERP2025.Domain.Core.Enums
{
	public enum LinkType
	{
		None,

		// Finance
		BudgetPlanReview,
		BudgetPlanDetail,
		BudgetPlanApproved,

		// Expense 
		ExpensePaymentDetail,
		OutgongPaymentPending,

		// Request
		RequestDetail,

		// Others
		Dashboard
	}
}
