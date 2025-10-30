namespace ThaiTuanERP2025.Application.Finance.Budgets.Requests
{
	public sealed record BudgetPeriodRequest
	{
		public int Month { get; set; }
		public int Year { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}
