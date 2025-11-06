using AutoMapper;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.BudgetCodes
{
	public class BudgetCodeMappingProfile : Profile
	{
		public BudgetCodeMappingProfile()
		{
			CreateMap<BudgetCode, BudgetCodeDto>()
				.ForMember(d => d.BudgetGroupName, o => o.MapFrom(s => s.BudgetGroup.Name));
		}
	}
}
