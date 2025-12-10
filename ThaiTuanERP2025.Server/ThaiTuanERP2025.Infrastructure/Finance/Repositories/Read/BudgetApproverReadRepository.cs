using AutoMapper;
using ThaiTuanERP2025.Application.Finance.BudgetApprovers;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories.Read
{
	public class BudgetApproverReadRepository : BaseReadRepository<BudgetApprover, BudgetApproverDto>, IBudgetApproverReadRepository
	{
		public BudgetApproverReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapperConfig) : base(dbContext, mapperConfig) { }
	}
}
