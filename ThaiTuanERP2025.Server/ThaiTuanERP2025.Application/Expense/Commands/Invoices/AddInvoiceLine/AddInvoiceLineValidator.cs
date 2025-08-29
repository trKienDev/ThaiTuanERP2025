using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Expense.Commands.Invoices.AddInvoiceLine
{
	public sealed class AddInvoiceLineValidator : AbstractValidator<AddInvoiceLineRequest>
	{
		public AddInvoiceLineValidator() {
			RuleFor(x => x.InvoiceId).NotEmpty();
			RuleFor(x => x.ItemName).NotEmpty().MaximumLength(250);
			RuleFor(x => x.Quantity).GreaterThan(0);
			RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
			RuleFor(x => x.DiscountRate).InclusiveBetween(0, 100).When(x => x.DiscountRate.HasValue);
			RuleFor(x => x.TaxRatePercent).InclusiveBetween(0, 100).When(x => x.TaxRatePercent.HasValue);
		}
	}
}
