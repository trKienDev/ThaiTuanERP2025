using AutoMapper;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Finance.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories.Write
{
	public sealed class BudgetTransactionWriteRepository : BaseWriteRepository<BudgetTransaction>, IBudgetTransactionWriteRepository
	{
		public BudgetTransactionWriteRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider mapperConfig) : base(dbContext, mapperConfig)
		{

		}
	}
}
