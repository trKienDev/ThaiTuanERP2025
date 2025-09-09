using MediatR;
using ThaiTuanERP2025.Application.Common.Models;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.Invoices.GetMyInvoices
{
	public record GetMyInvoicesQuery(int Page = 1, int PageSize = 20) : IRequest<PagedResult<InvoiceDto>>;
}
