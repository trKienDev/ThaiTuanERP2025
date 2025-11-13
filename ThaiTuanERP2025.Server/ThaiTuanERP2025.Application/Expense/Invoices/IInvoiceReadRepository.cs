using ThaiTuanERP2025.Application.Shared.Models;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Shared.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Invoices
{
	public interface IInvoiceReadRepository : IBaseWriteRepository<Invoice>
	{
		Task<PagedResult<InvoiceDto>> GetInvoicesPagedAsync(int page, int pageSize, string? keyword, CancellationToken cancellationToken);
		Task<PagedResult<InvoiceDto>> GetByCreatorPagedAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken);
	}
}
