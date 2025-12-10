using AutoMapper;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Account.Repositories;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories.Write
{
	public class DepartmentWriteRepository : BaseWriteRepository<Department>, IDepartmentWriteRepository
	{
		private readonly ThaiTuanERP2025DbContext _dbContext;
		public DepartmentWriteRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}
	}
}
