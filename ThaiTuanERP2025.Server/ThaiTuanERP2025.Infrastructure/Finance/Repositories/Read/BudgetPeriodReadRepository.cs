using AutoMapper;
using ThaiTuanERP2025.Application.Finance.BudgetPeriods;
using ThaiTuanERP2025.Application.Finance.Budgets.DTOs;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories.Read
{
	public class BudgetPeriodReadRepository : BaseReadRepository<BudgetPeriod, BudgetPeriodDto>, IBudgetPeriodReadRepository
	{
		public BudgetPeriodReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapper) 
			: base(dbContext, mapper) { }
	}
}
