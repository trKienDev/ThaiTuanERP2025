using System.Runtime.InteropServices;
using ThaiTuanERP2025.Application.Account.Departments;
using ThaiTuanERP2025.Application.Finance.BudgetCodes;
using ThaiTuanERP2025.Application.Finance.BudgetPeriods;
using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans.Contracts
{
	public sealed record BudgetPlanDto
	{
		public Guid Id { get; init; }
		
		public Guid DepartmentId { get; init; }
		public DepartmentBriefDto Department { get; init; } = default!;
		
		public Guid BudgetPeriodId { get; init; }
		public BudgetPeriodDto BudgetPeriod { get; init; } = default!;

		public decimal TotalAmount { get; init; } = default!;
		public bool IsReviewed { get; init; }
		public bool IsApproved { get; init; }

		public DateTime CreatedAt { get; init; }

		public Guid SelectedReviewerId { get; init; }
		public bool CanReview { get; set; } 

		public Guid SelectedApproverId { get; init; }
		public bool CanApprove { get; set; }	

		public BudgetPlanStatus Status { get; init; }

		public DateTime? DueAt { get; init; }

		public IReadOnlyList<BudgetPlanDetailDto> Details { get; init; } = [];
	}

	public sealed record BudgetPlanDetailDto {
		public Guid Id { get; init; }
		public Guid BudgetCodeId { get; init; }
		public BudgetCodeDto BudgetCode { get; init; } = default!;
		public decimal Amount { get; init; }
	}
}
