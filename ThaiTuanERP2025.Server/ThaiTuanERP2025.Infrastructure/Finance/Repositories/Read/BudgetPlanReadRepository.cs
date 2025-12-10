using AutoMapper;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Application.Finance.BudgetPlans.Repositories;
using ThaiTuanERP2025.Application.Finance.BudgetPlans.Contracts;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories.Read
{
	public class BudgetPlanReadRepository : BaseReadRepository<BudgetPlan, BudgetPlanDto>, IBudgetPlanReadRepository
	{
		public BudgetPlanReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }
	}
}
