namespace ThaiTuanERP2025.Application.Finance.BudgetPlans
{
	public sealed record BudgetPlanDetailRequest(Guid BudgetCodeId, decimal Amount, string? Note);
	public sealed record BudgetPlanRequest
	{
		public Guid DepartmentId { get; set; }
		public Guid BudgetPeriodId { get; set; }
		public Guid SelectedReviewerId { get; set; }
		public Guid SelectedApproverId { get; set; }
		public IReadOnlyList<BudgetPlanDetailRequest> Details { get; set; } = new List<BudgetPlanDetailRequest>();
	}
}
