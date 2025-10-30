using AutoMapper;
using ThaiTuanERP2025.Application.Account.RBAC.Dtos;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.RBAC.MappingProfiles
{
	public class RoleMappingProfile : Profile
	{
		public RoleMappingProfile() {
			CreateMap<Role, RoleDto>();
		}
	}
}
