using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Commands.LedgerAccounts.CreateLedgerAccount
{
	public class CreateLedgerAccountValidator : AbstractValidator<CreateLedgerAccountCommand>
	{
		public CreateLedgerAccountValidator() {
			RuleFor(x => x.Number).NotEmpty().WithMessage("Số tài khoản không được để trống.")
				.MaximumLength(20).WithMessage("Số tài khoản không được vượt quá 20 ký tự.");
			RuleFor(x => x.Name).NotEmpty().WithMessage("Tên tài khoản không được để trống.")
				.MaximumLength(100).WithMessage("Tên tài khoản không được vượt quá 100 ký tự.");
		}
	}
}
