using ThaiTuanERP2025.Application.Account.Departments;
using ThaiTuanERP2025.Application.Finance.BudgetCodes;
using ThaiTuanERP2025.Application.Finance.BudgetPeriods;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans
{
	public sealed record BudgetPlanDto
	{
		public Guid Id { get; init; }
		
		public Guid DepartmentId { get; init; }
		public DepartmentBriefDto Department { get; init; } = default!;
		
		public Guid BudgetCodeId { get; init; }
		public BudgetCodeDto BudgetCode { get; init; } = default!;
		
		public Guid BudgetPeriodId { get; init; }
		public BudgetPeriodDto BudgetPeriod { get; init; } = default!;

		public decimal Amount { get; init; } = default!;
		public bool IsReviewed { get; init; }
		public bool IsApproved { get; init; }
		public DateTime CreatedDate { get; init; }
	}
}
