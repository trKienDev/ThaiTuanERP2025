namespace ThaiTuanERP2025.Domain.Core.Enums
{
	public enum NotificationLinkType
	{
		None = 0,

		// Budget 
		BudgetPlanReview,
		BudgetPlanDetail,

		// Expense 
		ExpensePaymentDetail,

		// User request module
		RequestDetail,

		// Others
		Dashboard
	}
}
