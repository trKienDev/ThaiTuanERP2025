using AutoMapper;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Finance.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories.Write
{
	public class BudgetPlanDetailWriteRepository : BaseWriteRepository<BudgetPlanDetail>, IBudgetPlanDetailWriteRepository
	{
		public BudgetPlanDetailWriteRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider)
		{
		}
	}
}
