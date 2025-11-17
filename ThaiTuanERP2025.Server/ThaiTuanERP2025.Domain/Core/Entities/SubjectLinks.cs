namespace ThaiTuanERP2025.Domain.Core.Entities
{
	public static class SubjectLinks
	{
		public static string BudgetPlanDetail(Guid planId) 
			=> $"/finance/budgets-shell/budget-plans?openBudgetPlanId={planId}";

		public static string ExpensePaymentDetail(Guid id) => $"/expense-payments/{id}";

		public static string RequestDetail(Guid id) => $"/requests/{id}";

		public static string Dashboard() => "/dashboard";
	}
}
