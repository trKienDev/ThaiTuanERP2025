using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.Invoices.CreateInvoiceDraft
{
	public sealed record CreateInvoiceRequest(
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

	public sealed record CreateInvoiceCommand(CreateInvoiceRequest Request) : IRequest<InvoiceDto>;
}
