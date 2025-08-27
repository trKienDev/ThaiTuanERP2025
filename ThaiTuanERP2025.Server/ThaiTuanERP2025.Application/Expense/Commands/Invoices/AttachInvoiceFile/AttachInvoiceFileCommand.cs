using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.Invoices.AttachInvoiceFile
{
	public sealed record AttachInvoiceFileRequest(Guid InvoiceId, Guid FileId, bool IsMain);
	public sealed record AttachInvoiceFileCommand(AttachInvoiceFileRequest Request) : IRequest<InvoiceDto>;
}
