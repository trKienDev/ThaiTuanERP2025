using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Common.Repositories;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.BudgetCodes
{
	public interface IBudgetCodeReadRepository : IBaseRepository<BudgetCode>
	{
		Task<List<BudgetCodeWithAmountDto>> GetWithAmountForPeriodAsync(int year, int month, Guid departmentId, CancellationToken cancellationToken);
	}
}
