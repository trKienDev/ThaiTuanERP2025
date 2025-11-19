using ThaiTuanERP2025.Domain.Shared.Repositories;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Repositories
{
	public interface ILedgerAccountWriteRepository : IBaseWriteRepository<LedgerAccount>
	{
		Task<bool> NumberExistsAsync(string number, Guid? excludeId = null, CancellationToken cancellationToken = default);
		Task<List<LedgerAccount>> GetSubtreeAsync(string pathPrefix, bool asNoTracking = true, CancellationToken cancellationToken = default);
	}
}
