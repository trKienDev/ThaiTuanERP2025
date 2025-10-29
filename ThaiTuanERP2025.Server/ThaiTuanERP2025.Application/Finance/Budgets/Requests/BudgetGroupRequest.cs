namespace ThaiTuanERP2025.Application.Finance.Budgets.Requests
{
	public sealed record BudgetGroupRequest {
		public string Name { get; init; } = string.Empty;
		public string Code { get; init; } = string.Empty;
	}
}
