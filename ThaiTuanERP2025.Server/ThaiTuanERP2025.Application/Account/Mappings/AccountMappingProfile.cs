using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
				.ForMember(dest => dest.Email,
					opt => opt.MapFrom(src => src.Email != null ? src.Email.Value : null))
				.ForMember(dest => dest.Phone,
					opt => opt.MapFrom(src => src.Phone != null ? src.Phone.Value : null));

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
