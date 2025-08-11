using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Models;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Repositories
{
	public interface IBankAccountReadRepository
	{
		Task<PagedResult<BankAccountDto>> SearchPagedAsync(bool? onlyActive, Guid? departmentId, int page, int pageSize, CancellationToken cancellationToken = default);
		Task<BankAccountDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

	}
}
