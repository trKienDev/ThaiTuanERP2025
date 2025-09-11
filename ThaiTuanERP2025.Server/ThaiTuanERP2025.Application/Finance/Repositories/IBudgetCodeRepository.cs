using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.Repositories
{
	public interface IBudgetCodeRepository : IBaseRepository<BudgetCode>
	{
		Task<List<BudgetCodeWithAmountDto>> GetWithAmountForPeriodAsync(int year, int month, Guid departmentId, CancellationToken cancellationToken);
	}
}
