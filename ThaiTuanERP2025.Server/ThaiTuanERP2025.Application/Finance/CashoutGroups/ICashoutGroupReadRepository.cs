using ThaiTuanERP2025.Application.Finance.CashoutGroups.Contracts;
using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.CashoutGroups
{
	public interface ICashoutGroupReadRepository : IBaseReadRepository<CashoutGroup, CashoutGroupDto>
	{
		Task<int> GetMaxOrderNumberAsync(Guid? parentId, CancellationToken cancellationToken);
	}
}
