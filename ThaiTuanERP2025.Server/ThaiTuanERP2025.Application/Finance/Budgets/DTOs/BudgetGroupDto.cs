namespace ThaiTuanERP2025.Application.Finance.Budgets.DTOs
{
	public sealed record BudgetGroupDto
	{
		public Guid Id { get; init; }
		public string Name { get; init; } = string.Empty;
		public string Code { get; init; } = string.Empty;
	}
}
