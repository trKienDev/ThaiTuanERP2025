using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Finance.BudgetPeriods;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories.Read
{
	public class BudgetPeriodReadRepository : BaseReadRepository<BudgetPeriod, BudgetPeriodDto>, IBudgetPeriodReadRepository
	{
		public BudgetPeriodReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapper)  : base(dbContext, mapper) { }

		public async Task<IReadOnlyList<BudgetPeriodDto>> GetAvailableBudgetPeriods(CancellationToken cancellationToken)
		{
			var now = DateTime.UtcNow;

			return await Query()
				.Where( 
					bp => !bp.IsDeleted
					&& bp.Year == now.Year
					&& bp.Month == now.Month
				).OrderBy(bp => bp.Month)
				.ProjectTo<BudgetPeriodDto>(_mapperConfig)
				.ToListAsync(cancellationToken);
		}

		public Task<List<Guid>> GetAvailablePeriodIdsAsync(CancellationToken cancellationToken)
		{
			var now = DateTime.UtcNow;

			return Query()
				.Where(bp => !bp.IsDeleted && bp.Month == now.Month && bp.Year == now.Year)
				.OrderBy(bp => bp.Month)
				.Select(bp => bp.Id)
				.ToListAsync(cancellationToken);
		}
	}
}
