using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Models;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.Invoices.GetInvoicesPaged
{
	public sealed record GetInvoicesPagedQuery(int Page = 1, int PageSize = 10, string? Keyword = null) : IRequest<PagedResult<InvoiceDto>>;
}
