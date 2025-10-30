using AutoMapper;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.RBAC.Roles
{
	public class RoleMappingProfile : Profile
	{
		public RoleMappingProfile()
		{
			CreateMap<Role, RoleDto>();
		}
	}
}
