using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Models;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Repositories
{
	public interface IInvoiceRepository : IBaseRepository<Invoice>
	{
		Task<PagedResult<InvoiceDto>> GetInvoicesPagedAsync(int page, int pageSize, string? keyword, CancellationToken cancellationToken);
	}
}
