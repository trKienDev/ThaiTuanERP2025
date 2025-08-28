using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.Invoices.CreateInvoiceDraft
{
	public sealed record CreateInvoiceDraftRequest(
		string InvoiceNumber,
		string InvoiceName,
		DateTime IssueDate,
		DateTime? PaymentDate,
		string SellerName,
		string SellerTaxCode,
		string? SellerAddress,
		string? BuyerName,
		string? BuyerTaxCode,
		string? BuyerAddress,
		Guid? MainFileId
	);

	public sealed record CreateInvoiceDraftCommand(CreateInvoiceDraftRequest Request) : IRequest<InvoiceDto>;
}
