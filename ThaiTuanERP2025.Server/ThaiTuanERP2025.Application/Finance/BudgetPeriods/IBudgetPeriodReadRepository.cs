using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.BudgetPeriods
{
	public interface IBudgetPeriodReadRepository : IBaseReadRepository<BudgetPeriod, BudgetPeriodDto>
	{
		Task<List<Guid>> GetAvailablePeriodIdsAsync(CancellationToken cancellationToken);
		Task<IReadOnlyList<BudgetPeriodDto>> GetAvailableBudgetPeriods(CancellationToken cancellationToken);
	}
}
