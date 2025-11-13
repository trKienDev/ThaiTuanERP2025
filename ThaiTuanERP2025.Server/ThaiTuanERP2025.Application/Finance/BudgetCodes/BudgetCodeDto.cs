namespace ThaiTuanERP2025.Application.Finance.BudgetCodes
{
	public sealed record BudgetCodeDto
	{
		public Guid Id { get; set; }
		public string Code { get; set; } = null!;
		public string Name { get; set; } = null!;
		public Guid BudgetGroupId { get; set; }

		public string? BudgetGroupName { get; set; }

		public bool IsActive { get; set; } = true;
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
	}

	public record BudgetCodeWithAmountDto (
		Guid Id, 
		string Code, 
		string Name, 
		int Year,
		int Month,
		Guid? BudgetPlanId,
		decimal? Amount,
		string? BudgetGroupName
	);
}
