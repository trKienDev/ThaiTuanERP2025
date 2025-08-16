using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Commands.BankAccounts.DeleteBankAccount
{
	public class DeleteBankAccountCommandValidator : AbstractValidator<DeleteBankAccountCommand>
	{
		public DeleteBankAccountCommandValidator() {
			RuleFor(x => x.Id).NotEmpty().WithMessage("Id không hợp lệ");
		}
	}
}
