using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.Repositories
{
	public interface IBankAccountRepository : IBaseRepository<BankAccount>
	{
		Task<bool> ExistsDuplicateAsync(string accountNumber, string bankName, Guid? departmentId, string? customerName, Guid? excludeId = null, CancellationToken cancellationToken = default);
	}
}
