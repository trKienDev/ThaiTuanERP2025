using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Repositories
{
	public interface IBankAccountRepository : IBaseRepository<BankAccount>
	{
		Task<bool> ExistsForUserAsync(Guid userId, CancellationToken cancellationToken);
		Task<BankAccount?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
		Task<IReadOnlyList<BankAccount>> ListBySupplierIdAsync(Guid supplierId, CancellationToken cancellationToken);
	}
}
