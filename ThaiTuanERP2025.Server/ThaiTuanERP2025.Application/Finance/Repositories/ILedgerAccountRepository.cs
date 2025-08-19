using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.Repositories
{
	public interface ILedgerAccountRepository : IBaseRepository<LedgerAccount>
	{
		Task<bool> NumberExistsAsync(string number, Guid? excludeId = null, CancellationToken cancellationToken = default);
		Task<List<LedgerAccount>> GetSubtreeAsync(string pathPrefix, bool asNoTracking = true, CancellationToken cancellationToken = default);
	}
}
