using ThaiTuanERP2025.Application.Account.Departments;
using ThaiTuanERP2025.Application.Finance.BudgetCodes;
using ThaiTuanERP2025.Application.Finance.BudgetPeriods;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans
{
	public sealed record BudgetPlanDto
	{
		public Guid Id { get; }
		
		public Guid DepartmentId { get; }
		public DepartmentBriefDto Departmemt { get; } = default!;
		
		public Guid BudgetCodeId { get; }
		public BudgetCodeDto BudgetCode { get; } = default!;
		
		public Guid BudgetPeriodId { get;  }
		public BudgetPeriodDto BudgetPeriod { get; } = default!;

		public decimal Amount { get; } = default!;
	}
}
