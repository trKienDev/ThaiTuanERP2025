using AutoMapper;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Shared.Utils;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans
{
	public class BudgetPlanMappingProfile : Profile
	{
		public BudgetPlanMappingProfile()
		{
			CreateMap<BudgetPlan, BudgetPlanDto>()
				.ForMember(d => d.Department, o => o.MapFrom(s => s.Department))
				.ForMember(d => d.BudgetCode, o => o.MapFrom(s => s.BudgetCode))
				.ForMember(d => d.BudgetPeriod, o => o.MapFrom(s => s.BudgetPeriod))
				.ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => TimeZoneConverter.ToVietnamTime(src.CreatedDate)))
				.ForMember(d => d.SelectedReviewerId, opt => opt.MapFrom(s => s.SelectedReviewerId))
				.ForMember(d => d.CanReview, opt => opt.Ignore()); 
		}
	}
}
