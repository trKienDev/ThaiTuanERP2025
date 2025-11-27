namespace ThaiTuanERP2025.Domain.Core.Entities
{
	public static class SubjectLinks
	{
		public static string BudgetPlanDetail(Guid planId)  => $"/finance/budgets-shell/budget-plans?openBudgetPlanId={planId}";
		public static string ExpensePaymentDetail(Guid id) => $"/expense-payments/{id}";
		public static string ExpensePaymentApprove(Guid id) => $"/expense-payment-detail/{id}";

		public static string Dashboard() => "/dashboard";
	}
}
