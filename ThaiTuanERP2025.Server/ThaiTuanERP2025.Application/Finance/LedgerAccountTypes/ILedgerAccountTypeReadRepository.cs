using ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Contracts;
using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.LedgerAccountTypes
{
	public interface ILedgerAccountTypeReadRepository : IBaseReadRepository<LedgerAccountType, LedgerAccountTypeDto>
	{
	}
}
