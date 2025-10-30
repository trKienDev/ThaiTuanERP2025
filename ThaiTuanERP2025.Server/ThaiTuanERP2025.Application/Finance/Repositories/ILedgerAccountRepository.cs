using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.Repositories
{
	public interface ILedgerAccountRepository : IBaseRepository<LedgerAccount>
	{
		Task<bool> NumberExistsAsync(string number, Guid? excludeId = null, CancellationToken cancellationToken = default);
		Task<List<LedgerAccount>> GetSubtreeAsync(string pathPrefix, bool asNoTracking = true, CancellationToken cancellationToken = default);
		Task<List<LedgerAccountLookupDto>> LookupAsync(string? keyword, int take, CancellationToken cancellationToken = default);
	}
}
