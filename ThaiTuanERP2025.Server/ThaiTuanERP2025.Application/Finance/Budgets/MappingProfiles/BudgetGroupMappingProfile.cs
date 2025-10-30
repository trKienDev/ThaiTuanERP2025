using AutoMapper;
using ThaiTuanERP2025.Application.Finance.Budgets.DTOs;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.Budgets.MappingProfiles
{
	public class BudgetGroupMappingProfile : Profile
	{
		public BudgetGroupMappingProfile() {
			CreateMap<BudgetGroup, BudgetGroupDto>();
		}
	}
}
