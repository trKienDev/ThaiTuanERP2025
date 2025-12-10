using AutoMapper;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Departments
{
	public class DepartmentMappingProfile : Profile
	{
		public DepartmentMappingProfile() {
			CreateMap<Department, DepartmentBriefDto>();

			CreateMap<Department, DepartmentDto>()
				.ForCtorParam(nameof(DepartmentDto.Id), opt => opt.MapFrom(src => src.Id))
				.ForCtorParam(nameof(DepartmentDto.Name), opt => opt.MapFrom(src => src.Name))
				.ForCtorParam(nameof(DepartmentDto.Code), opt => opt.MapFrom(src => src.Code))
				.ForCtorParam(nameof(DepartmentDto.PrimaryManager),
					opt => opt.MapFrom(src => 
						src.Managers.Where(m => m.IsPrimary)
							.Select(m => m.User)        // -> User
							.FirstOrDefault())
				)         // -> User?
				.ForCtorParam(nameof(DepartmentDto.ViceManagers),
					opt => opt.MapFrom(src =>
						src.Managers.Where(m => !m.IsPrimary)
							.Select(m => m.User)));
				}
	}
}
