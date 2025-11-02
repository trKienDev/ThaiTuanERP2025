using AutoMapper;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Account.Repositories;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories.Write
{
	public class RoleWriteRepository : BaseWriteRepository<Role>, IRoleWriteRepository
	{
		private readonly ThaiTuanERP2025DbContext _dbContext;
		public RoleWriteRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}
	}
}
