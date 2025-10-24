using AutoMapper;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories
{
	public class RolePermissionRepository : BaseRepository<RolePermission>, IRolePermissionRepository
	{
		private readonly ThaiTuanERP2025DbContext _dbContext;
		public RolePermissionRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}
	}
}
