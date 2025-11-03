using AutoMapper;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Permissions
{
	public class PermissionMappingProfile : Profile
	{
		public PermissionMappingProfile()
		{
			CreateMap<Permission, PermissionDto>();
		}
	}
}
