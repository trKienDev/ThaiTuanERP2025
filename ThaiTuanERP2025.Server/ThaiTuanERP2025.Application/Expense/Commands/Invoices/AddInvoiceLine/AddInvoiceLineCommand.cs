using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.Invoices.AddInvoiceLine
{
	public sealed record AddInvoiceLineRequest(
		Guid InvoiceId,
		string ItemName,
		string? Unit,
		decimal Quantity,
		decimal UnitPrice,
		decimal? DiscountRate,
		decimal? DiscountAmount,
		Guid? TaxId,
		Guid? WHTTypeId
	);
	public sealed record AddInvoiceLineCommand(AddInvoiceLineRequest Request) : IRequest<InvoiceDto>;
}
