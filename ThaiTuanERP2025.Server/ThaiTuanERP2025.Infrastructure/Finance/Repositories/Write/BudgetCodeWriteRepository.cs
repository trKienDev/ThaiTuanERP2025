using AutoMapper;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Finance.Repositories;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories
{
	public class BudgetCodeWriteRepository : BaseWriteRepository<BudgetCode>, IBudgetCodeWriteRepository {
		private readonly ThaiTuanERP2025DbContext _db;
		public BudgetCodeWriteRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider)
		{
			_db = dbContext;
		}
		
	}
}
