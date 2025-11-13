namespace ThaiTuanERP2025.Domain.Core.Entities
{
	public static class NotificationLinks
	{
		public static string BudgetPlanReview(Guid planId) => $"/budget-plans/{planId}/review";

		public static string BudgetPlanDetail(Guid planId) => $"/budget-plans/{planId}";

		public static string ExpensePaymentDetail(Guid id) => $"/expense-payments/{id}";

		public static string RequestDetail(Guid id) => $"/requests/{id}";

		public static string Dashboard() => "/dashboard";
	}
}
