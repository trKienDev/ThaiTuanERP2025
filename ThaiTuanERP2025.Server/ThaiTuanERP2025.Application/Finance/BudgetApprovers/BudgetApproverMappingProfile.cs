using AutoMapper;
using ThaiTuanERP2025.Application.Account.Departments;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.BudgetApprovers
{
	public class BudgetApproverMappingProfile : Profile
	{
		public BudgetApproverMappingProfile() {
			CreateMap<BudgetApproverDepartment, DepartmentBriefDto>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Department.Id))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Department.Name))
				.ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Department.Code));

			CreateMap<BudgetApprover, BudgetApproverDto>()
				.ForMember(dest => dest.Departments, opt => opt.MapFrom(src => src.Departments));
		}
	}
}
