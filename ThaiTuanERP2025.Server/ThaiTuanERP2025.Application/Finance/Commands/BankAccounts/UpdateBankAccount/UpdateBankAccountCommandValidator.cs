using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.Validators;

namespace ThaiTuanERP2025.Application.Finance.Commands.BankAccounts.UpdateBankAccount
{
	public class UpdateBankAccountCommandValidator : AbstractValidator<UpdateBankAccountCommand>
	{
		public UpdateBankAccountCommandValidator() {
			Include(new BankAccountOwnerValidator<UpdateBankAccountCommand>(
				x => x.DepartmentId,
				x => x.CustomerName
			));

			RuleFor(x => x.Id).NotEmpty().WithMessage("Id không được để trống");
			RuleFor(x => x.AccountNumber).NotEmpty().WithMessage("Số tài khoản không được bỏ trống")
				.MaximumLength(50).WithMessage("Số tài khoản không vượt quá 50 ký tự");
			RuleFor(x => x.BankName).NotEmpty().WithMessage("Tên ngân hàng không được bỏ trống")
				.MaximumLength(150).WithMessage("Tên ngân hàng không được vượt qua 150 ký tự");
			RuleFor(x => x.AccountHolder).MaximumLength(150).WithMessage("Tên tài khoản không vượt quá 150 ký tự");
			RuleFor(x => x.CustomerName).MaximumLength(200).WithMessage("Tên khách hàng không vượt quá 200 ký tự");


		}
	}
}
