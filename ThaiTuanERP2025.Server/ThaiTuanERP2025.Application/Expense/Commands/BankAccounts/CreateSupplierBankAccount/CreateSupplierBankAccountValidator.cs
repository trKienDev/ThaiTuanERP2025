using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.BankAccounts.CreateSupplierBankAccount
{
	public sealed class CreateSupplierBankAccountValidator : AbstractValidator<CreateSupplierBankAccountRequest>
	{
		public CreateSupplierBankAccountValidator() {
			RuleFor(x => x.SupplierId)
				.NotEmpty().WithMessage("Id nhà cung cấp không được để trống");
			RuleFor(x => x.BankName)
				.NotEmpty().WithMessage("Tên ngân hàng không được để trống")
				.MaximumLength(128).WithMessage("Tên tài khoản ngân hàng không vượt quá 128 ký tự");
			RuleFor(x => x.AccountNumber)
				.NotEmpty().WithMessage("Số tài khoản ngân hàng không được để trống")
				.MaximumLength(64).WithMessage("Số tài khoản ngân hàng không vượt quá 64 ký tự");
			RuleFor(x => x.BeneficiaryName)
				.NotEmpty().WithMessage("Tên người thụ hưởng không được để trống")
				.MaximumLength(128).WithMessage("Tên người thụ hưởng không vượt quá 128 ký tự");
		}
	}
}
