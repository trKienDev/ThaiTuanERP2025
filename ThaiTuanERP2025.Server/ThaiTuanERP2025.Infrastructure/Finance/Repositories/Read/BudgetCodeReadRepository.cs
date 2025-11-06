using AutoMapper;
using ThaiTuanERP2025.Application.Finance.BudgetCodes;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories.Read
{
	public class BudgetCodeReadRepository : BaseReadRepository<BudgetCode, BudgetCodeDto>, IBudgetCodeReadRepository
	{
		public BudgetCodeReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }
	}
}
