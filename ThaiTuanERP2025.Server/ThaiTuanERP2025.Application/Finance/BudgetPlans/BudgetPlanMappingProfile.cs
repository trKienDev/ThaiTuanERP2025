using AutoMapper;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans
{
	public class BudgetPlanMappingProfile : Profile
	{
		public BudgetPlanMappingProfile()
		{
			CreateMap<BudgetPlan, BudgetPlanDto>();
		}
	}
}
