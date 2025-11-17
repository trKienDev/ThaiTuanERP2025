using ThaiTuanERP2025.Application.Finance.BudgetPlans.Contracts;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans.Services
{
	public interface IBudgetPlanPermissionService
	{
		void ApplyPermissions(BudgetPlanDto dto, Guid userId);
		void ApplyPermissions(IEnumerable<BudgetPlanDto> dtos, Guid userId);
	}

	public class BudgetPlanPermissionService : IBudgetPlanPermissionService
	{
		public void ApplyPermissions(BudgetPlanDto dto, Guid userId)
		{
			dto.CanReview = dto.SelectedReviewerId == userId;
			dto.CanApprove = dto.SelectedApproverId == userId;
		}

		public void ApplyPermissions(IEnumerable<BudgetPlanDto> dtos, Guid userId)
		{
			foreach (var dto in dtos)
			{
				ApplyPermissions(dto, userId);
			}
		}
	}
}
