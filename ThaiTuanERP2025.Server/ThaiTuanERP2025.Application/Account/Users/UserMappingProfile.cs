using AutoMapper;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Users
{
	public class UserMappingProfile : Profile
	{
		public UserMappingProfile() {
			CreateMap<User, UserDto>()
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email != null ? src.Email.Value : null))
				.ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone != null ? src.Phone.Value : null))
				.ForMember(dest => dest.AvatarFileId, opt => opt.MapFrom(src => src.AvatarFileId))
				.ForMember(d => d.Roles, o => o.Ignore())
				.ForMember(d => d.Managers, o => o.Ignore());

			CreateMap<User, UserBriefDto>();

			CreateMap<User, UserBriefAvatarDto>();

			CreateMap<UserDto, UserBriefAvatarDto>()
				.ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
				.ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName))
				.ForMember(d => d.Username, o => o.MapFrom(s => s.Username))
				.ForMember(d => d.EmployeeCode, o => o.MapFrom(s => s.EmployeeCode))
				.ForMember(d => d.AvatarFileId, o => o.MapFrom(s => s.AvatarFileId))
				.ForMember(d => d.AvatarFileObjectKey, o => o.MapFrom(s => s.AvatarFileObjectKey));

			CreateMap<User, UserInforDto>()
				.ForMember(dest => dest.DepartmentName, 
					opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null)
				)
				.ForMember(dest => dest.RoleNames, 
					opt => opt.MapFrom(src =>  src.UserRoles.Where(ur => ur.Role != null)
														.Select(ur => ur.Role.Name))
				);
		}
	}
}
