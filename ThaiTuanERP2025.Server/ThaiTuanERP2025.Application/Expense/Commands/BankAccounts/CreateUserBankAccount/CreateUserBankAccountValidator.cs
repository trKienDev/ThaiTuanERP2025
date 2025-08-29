using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.BankAccounts.CreateUserBankAccount
{
	public sealed class CreateUserBankAccountValidator : AbstractValidator<CreateUserBankAccountRequest>
	{
		public CreateUserBankAccountValidator() {
			RuleFor(x => x.UserId)
				.NotEmpty().WithMessage("Id người dùng không được để trống");
			RuleFor(x => x.BankName)
				.NotEmpty().WithMessage("Tên ngân hàng không được để trống")
				.MaximumLength(128).WithMessage("Tên ngân hàng không vượt quá 128 ký tự");
			RuleFor(x => x.AccountNumber)
				.NotEmpty().WithMessage("Số tài khoản không được để trống")
				.MaximumLength(64).WithMessage("Số tài khoản không được vượt quá 64 ký tự");
			RuleFor(x => x.BeneficiaryName)
				.NotEmpty().WithMessage("Tên người thụ hưởng không được để trống")
				.MaximumLength(128).WithMessage("Tên người thụ hưởng không vượt quá 128 ký tự");
		}
	}
}
