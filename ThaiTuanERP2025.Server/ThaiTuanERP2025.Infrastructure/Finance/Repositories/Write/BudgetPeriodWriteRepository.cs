using AutoMapper;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Finance.Repositories;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories.Write
{
	public class BudgetPeriodWriteRepository : BaseWriteRepository<BudgetPeriod>, IBudgetPeriodWriteRepository 
	{
		public BudgetPeriodWriteRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider)
		{
			// Constructor logic if needed
		}
		// Implement any custom methods for BudgetPeriod here
	}
}
