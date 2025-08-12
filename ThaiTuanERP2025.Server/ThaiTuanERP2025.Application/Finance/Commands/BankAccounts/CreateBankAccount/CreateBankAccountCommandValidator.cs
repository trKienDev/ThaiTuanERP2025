using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.Validators;

namespace ThaiTuanERP2025.Application.Finance.Commands.BankAccounts.CreateBankAccount
{
	public class CreateBankAccountCommandValidator : AbstractValidator<CreateBankAccountCommand>
	{
		public CreateBankAccountCommandValidator() {
			Include(new BankAccountOwnerValidator<CreateBankAccountCommand>(
				x => x.OwnerName
			));

			RuleFor(x => x.AccountNumber).NotEmpty().WithMessage("Số tài khoản không được để trống")
				.MaximumLength(50).WithMessage("Số tài khoản không được vượt quá 50 ký tự");
			RuleFor(x => x.BankName).NotEmpty().WithMessage("Tên ngân hàng không được bỏ trống")
				.MaximumLength(150).WithMessage("Tên ngân hàng không được vượt quá 150 ký tự");
			RuleFor(x => x.AccountHolder).MaximumLength(150).WithMessage("Tên tài khoản không được vượt quá 150 ký tự");
			RuleFor(x => x.OwnerName).MaximumLength(200).WithMessage("Tên khách hàng không vượt quá 200 ký tự");
		}
	}
}
