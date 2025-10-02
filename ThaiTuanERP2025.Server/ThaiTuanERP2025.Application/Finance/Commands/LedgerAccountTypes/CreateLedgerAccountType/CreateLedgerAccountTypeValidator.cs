using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Commands.LedgerAccountTypes.CreateLedgerAccountType
{
	public class CreateLedgerAccountTypeValidator : AbstractValidator<CreateLedgerAccountTypeCommand>
	{
		public CreateLedgerAccountTypeValidator()
		{
			RuleFor(x => x.Code)
				.NotEmpty().WithMessage("Mã loại tài khoản không được để trống.")
				.MaximumLength(64).WithMessage("Mã loại tài khoản không được vượt quá 50 ký tự.");
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Tên loại tài khoản không được để trống.")
				.MaximumLength(250).WithMessage("Tên loại tài khoản không được vượt quá 250 ký tự.");
		}
	}
}
