using System.Linq.Expressions;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts.Contracts;
using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.LedgerAccounts
{
	public interface ILedgerAccountReadRepository : IBaseReadRepository<LedgerAccount, LedgerAccountDto>
	{
                Task<IReadOnlyList<LedgerAccountDto>> GetAllActiveAsync(CancellationToken cancellationToken = default);
                Task<IReadOnlyList<LedgerAccountTreeDto>> GetTreeAsync(CancellationToken cancellationToken = default);
                Task<LedgerAccountDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        }
}
