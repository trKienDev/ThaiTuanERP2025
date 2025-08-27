using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Expense.Commands.Invoices.CreateInvoiceDraft
{
	public sealed class CreateInvoiceDraftValidator : AbstractValidator<CreateInvoiceDraftRequest>
	{
		public CreateInvoiceDraftValidator() {
			RuleFor(x => x.InvoiceName).NotEmpty().MaximumLength(250);
			RuleFor(x => x.InvoiceNumber).NotEmpty().MaximumLength(50);
			RuleFor(x => x.IssueDate).NotEmpty();
			RuleFor(x => x.SellerName).NotEmpty().MaximumLength(255);
			RuleFor(x => x.SellerTaxCode).NotEmpty().MaximumLength(50);
			RuleFor(x => x.MainFileId).NotEmpty();
		}
	}
}
