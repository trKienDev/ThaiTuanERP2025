using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.Repositories
{
	public interface ISupplierRepository : IBaseRepository<Supplier>
	{
		Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
		Task<Supplier?> FindByCodeAsync(string code, CancellationToken cancellationToken = default);
		Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
		Task<(IReadOnlyList<Supplier> Items, int Total)> SearchAsync(string? keyword, bool? isActive, string? currency, int page, int pageSize, CancellationToken cancellationToken = default);
	}
}
