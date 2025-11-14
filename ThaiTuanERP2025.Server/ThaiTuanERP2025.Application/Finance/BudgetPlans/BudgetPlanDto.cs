using ThaiTuanERP2025.Application.Account.Departments;
using ThaiTuanERP2025.Application.Finance.BudgetCodes;
using ThaiTuanERP2025.Application.Finance.BudgetPeriods;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans
{
	public sealed record BudgetPlansByDepartmentDto
	{
		public Guid DepartmentId { get; set; }
		public string DepartmentName { get; set; } = default!;
		public int Year { get; set; }
		public int Month { get; set; }
		public decimal TotalAmount { get; set; }

		public IReadOnlyList<BudgetPlanDto> BudgetPlans { get; set; } = new List<BudgetPlanDto>();
	}

	public sealed record BudgetPlanDto
	{
		public Guid Id { get; init; }
		
		public Guid DepartmentId { get; set; }
		public DepartmentBriefDto Department { get; set; } = default!;
		
		public Guid BudgetCodeId { get; set; }
		public BudgetCodeDto BudgetCode { get; set; } = default!;
		
		public Guid BudgetPeriodId { get; set; }
		public BudgetPeriodDto BudgetPeriod { get; set; } = default!;

		public decimal Amount { get; set; } = default!;
		public bool IsReviewed { get; init; }
		public bool IsApproved { get; set; }
		public DateTime CreatedDate { get; set; }

		public Guid SelectedReviewerId { get; set; }
		public bool CanEdit { get; set; } // Dùng cho ui

	}
}
