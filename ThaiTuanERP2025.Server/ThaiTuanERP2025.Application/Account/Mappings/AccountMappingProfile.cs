using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Domain.Account.Entities;
using AutoMapper;
using ThaiTuanERP2025.Application.Account.Departments;

namespace ThaiTuanERP2025.Application.Account.Mappings
{
	public class AccountMappingProfile : Profile
	{
		public AccountMappingProfile() {
			// Nested mapping (nếu cần)
			CreateMap<Department, DepartmentDto>();

			CreateMap<Group, GroupDto>()
				.ForMember(dest => dest.Members, opt => opt.MapFrom(
					src => src.UserGroups.Select(ug => ug.User)
				))
				.ForMember(dest => dest.AdminName, opt => opt.MapFrom(
					src => src.UserGroups
						.Where(ug => ug.UserId == src.AdminId)
						.Select(ug => ug.User.FullName)
						.FirstOrDefault() ?? string.Empty
				))
				.ForMember(dest => dest.MemberCount, opt => opt.MapFrom(
					src => src.UserGroups.Count
				));
		}
	}
}
