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
		OutgoingPaymentPending,
		OutgoingPaymentApproved,
                OutgoingPaymentCreated,

                // Request
                RequestDetail,

		// Others
		Dashboard
	}
}
