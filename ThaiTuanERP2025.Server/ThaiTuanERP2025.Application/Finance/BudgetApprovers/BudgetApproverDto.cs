using ThaiTuanERP2025.Application.Account.Departments;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Finance.BudgetApprovers
{
	public sealed record BudgetApproverDto (
		Guid Id,
		User ApproverUser,
		int SlaHours,
		bool IsActive,
		IReadOnlyList<DepartmentBriefDto> Departments
	);
}
