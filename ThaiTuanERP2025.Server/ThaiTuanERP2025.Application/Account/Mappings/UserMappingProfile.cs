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
	public class UserMappingProfile : Profile
	{
		public UserMappingProfile() {
			// Mapping từ User sang UserDto --> ánh xạ 1 chiều
			CreateMap<User, UserDto>()
				.ForMember(dest => dest.Email,
					opt => opt.MapFrom(src => src.Email != null ? src.Email.Value : null))
				.ForMember(dest => dest.Phone,
					opt => opt.MapFrom(src => src.Phone != null ? src.Phone.Value : null));

			// Nested mapping (nếu cần)
			CreateMap<Department, DepartmentDto>();
		}
	}
}
