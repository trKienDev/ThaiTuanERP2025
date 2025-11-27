using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.BudgetTransasctions
{
	public interface IBudgetTransactionReadRepository : IBaseReadRepository<BudgetTransaction, BudgetTransactionDto>
	{
		Task<decimal> GetRemainingAsync(Guid budgetPlanDetailId, CancellationToken cancellationToken);
	}
}
