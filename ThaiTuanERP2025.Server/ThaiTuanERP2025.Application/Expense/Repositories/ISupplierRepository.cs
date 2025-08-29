using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Repositories
{
	public interface ISupplierRepository : IBaseRepository<Supplier>
	{
		Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
		Task<IReadOnlyList<Supplier>> SearchAsync(string? keywordm,  CancellationToken cancellationToken);
	}
}
