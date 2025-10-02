using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Models;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Repositories
{
	public interface IInvoiceRepository : IBaseRepository<Invoice>
	{
		Task<PagedResult<InvoiceDto>> GetInvoicesPagedAsync(int page, int pageSize, string? keyword, CancellationToken cancellationToken);
		Task<PagedResult<InvoiceDto>> GetByCreatorPagedAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken);
	}
}
