using AutoMapper;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.BudgetPeriods
{
	public class BudgetPeriodMappingProfile : Profile
	{
		public BudgetPeriodMappingProfile()
		{
			CreateMap<BudgetPeriod, BudgetPeriodDto>();
		}
	}
}
