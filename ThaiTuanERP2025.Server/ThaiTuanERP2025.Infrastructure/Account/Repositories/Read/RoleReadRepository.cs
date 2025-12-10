using AutoMapper;
using ThaiTuanERP2025.Application.Account.Roles;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories.Read
{
	public sealed class RoleReadRepository : BaseReadRepository<Role, RoleDto>, IRoleReadRepository
	{
		public RoleReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapper)
			: base(dbContext, mapper) { }
	}
}
