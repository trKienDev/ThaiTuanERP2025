using AutoMapper;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Permissions;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories.Read
{
	public class PermissionReadRepository : BaseReadRepository<Permission, PermissionDto>, IPermissionReadRepository
	{
		public PermissionReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapper)
			: base(dbContext, mapper) { }
	}
}
