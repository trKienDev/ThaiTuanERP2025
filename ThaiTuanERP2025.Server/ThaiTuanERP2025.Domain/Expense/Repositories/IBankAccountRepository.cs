using ThaiTuanERP2025.Domain.Common.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Repositories
{
	public interface IBankAccountRepository : IBaseRepository<BankAccount>
	{
		Task<bool> ExistsForUserAsync(Guid userId, CancellationToken cancellationToken);
		Task<BankAccount?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
		Task<IReadOnlyList<BankAccount>> ListBySupplierIdAsync(Guid supplierId, CancellationToken cancellationToken);
	}
}
