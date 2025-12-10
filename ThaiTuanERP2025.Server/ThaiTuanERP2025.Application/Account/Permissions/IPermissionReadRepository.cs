using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Permissions
{
	public interface IPermissionReadRepository : IBaseReadRepository<Permission, PermissionDto>
	{
	}
}
