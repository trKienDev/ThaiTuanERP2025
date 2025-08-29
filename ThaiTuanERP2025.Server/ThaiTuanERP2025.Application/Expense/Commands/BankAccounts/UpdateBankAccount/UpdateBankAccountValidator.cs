using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.BankAccounts.UpdateBankAccount
{
	public sealed class UpdateBankAccountValidator : AbstractValidator<UpdateBankAccountRequest>
	{
		public UpdateBankAccountValidator() {
			RuleFor(x => x.BankName)
				.NotEmpty().WithMessage("Tên ngân hàng không được để trống")
				.MaximumLength(128).WithMessage("Tên tài khoản ngân hàng không được vượt quá 128 ký tự");
			RuleFor(x => x.AccountNumber)
				.NotEmpty().WithMessage("Số tài khoản ngân hàng không được để trống")
				.MaximumLength(64).WithMessage("Số tài khoản ngân hàng không được vượt quá 64 ký tự");
			RuleFor(x => x.BeneficiaryName)
				.NotEmpty().WithMessage("Tên người thụ hưởng không được để trống")
				.MaximumLength(128).WithMessage("Tên người thụ hưởng không được vượt quá 128 ký tự");
		}
	}
}
