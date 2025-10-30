using ThaiTuanERP2025.Application.Common.Models;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Common.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Invoices
{
	public interface IInvoiceReadRepository : IBaseRepository<Invoice>
	{
		Task<PagedResult<InvoiceDto>> GetInvoicesPagedAsync(int page, int pageSize, string? keyword, CancellationToken cancellationToken);
		Task<PagedResult<InvoiceDto>> GetByCreatorPagedAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken);
	}
}
