using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.Invoices.ReplaceMainInvoiceFile
{
	public sealed record ReplaceMainInvoiceFileCommand(ReplaceMainInvocieFileRequest Request) : IRequest<InvoiceDto>;
	public sealed record ReplaceMainInvocieFileRequest(Guid InvoiceId, Guid NewFileId);
}
