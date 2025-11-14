using AutoMapper;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.BudgetPeriods
{
	public class BudgetPeriodMappingProfile : Profile
	{
		public BudgetPeriodMappingProfile()
		{
			CreateMap<BudgetPeriod, BudgetPeriodLookupDto>();
			
			CreateMap<BudgetPeriod, BudgetPeriodDto>()
				 .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive)); 
		}
	}
}
