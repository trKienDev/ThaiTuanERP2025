using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.Invoices.AddInvoiceFollowers
{
	public sealed record AddInvoiceFollowersCommand(Guid InvoiceId, Guid UserId) : IRequest<InvoiceDto>;
}
