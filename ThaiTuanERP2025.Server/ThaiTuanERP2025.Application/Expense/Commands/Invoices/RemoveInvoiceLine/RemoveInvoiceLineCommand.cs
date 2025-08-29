using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.Invoices.RemoveInvoiceLine
{
	public sealed record RemoveInvoiceLineCommand(Guid InvoiceId, Guid LineId) : IRequest<InvoiceDto>;
}
