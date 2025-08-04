using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Mappings
{
	public class GroupMappingProfile : Profile
	{
		public GroupMappingProfile() {
			// Map User -> UserDto
			CreateMap<User, UserDto>();

			// Map Group -> GroupDto
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
