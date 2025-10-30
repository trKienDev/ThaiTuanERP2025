using ThaiTuanERP2025.Domain.Common.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Repositories
{
	public interface ISupplierRepository : IBaseRepository<Supplier>
	{
		Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null, CancellationToken cancellationToken = default);
		Task<IReadOnlyList<Supplier>> SearchAsync(string? keywordm, CancellationToken cancellationToken);
	}
}
