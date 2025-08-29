using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Expense.Commands.Invoices.AttachInvoiceFile
{
	public sealed class AttachInvoiceFileValidator : AbstractValidator<AttachInvoiceFileRequest>
	{
		public AttachInvoiceFileValidator() {
			RuleFor(x => x.InvoiceId).NotEmpty();
			RuleFor(x => x.FileId).NotEmpty();
		}
	}
}
