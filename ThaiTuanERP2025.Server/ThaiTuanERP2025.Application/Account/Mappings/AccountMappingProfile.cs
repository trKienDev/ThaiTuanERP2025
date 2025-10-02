using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Domain.Account.Entities;
using AutoMapper;

namespace ThaiTuanERP2025.Application.Account.Mappings
{
	public class AccountMappingProfile : Profile
	{
		public AccountMappingProfile() {
			// Mapping từ User sang UserDto --> ánh xạ 1 chiều
			CreateMap<User, UserDto>()
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email != null ? src.Email.Value : null))
				.ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone != null ? src.Phone.Value : null))
				.ForMember(dest => dest.AvatarFileId, opt => opt.MapFrom(src => src.AvatarFileId))
				.ForMember(dest => dest.AvatarFileObjectKey, opt => opt.Ignore());
				
			// Nested mapping (nếu cần)
			CreateMap<Department, DepartmentDto>()
				.ForMember(dest => dest.Manager, opt => opt.MapFrom(src => src.ManagerUser));

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
