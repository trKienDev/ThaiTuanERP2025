namespace ThaiTuanERP2025.Application.Finance.DTOs
{
	public record BudgetPeriodDto
	{
		public Guid Id { get; init; }
		public int Year { get; init; }
		public int Month { get; init; }
		public DateTime BudgetPreparationDate { get; init; }
		public bool IsActive { get; init; }
	}

	public sealed record BudgetPeriodRequest {
		public int Month { get; set; }
		public int Year { get; set; }
		public DateTime BudgetPreparationDate { get; set; }
	}
}
