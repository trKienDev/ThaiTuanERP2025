using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Shared.Repositories;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.LedgerAccounts
{
	public interface ILedgerAccountReadRepository : IBaseWriteRepository<LedgerAccount>
	{
		Task<List<LedgerAccountLookupDto>> LookupAsync(string? keyword, int take, CancellationToken cancellationToken = default);
	}
}
