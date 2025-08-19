using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.Commands.LedgerAccountTypes.UpdateAccountType;

namespace ThaiTuanERP2025.Application.Finance.Commands.LedgerAccountTypes.UpdateLedgerAccountType
{
	public class UpdateLedgerAccountTypeValidator : AbstractValidator<UpdateLedgerAccountTypeCommand>
	{
		public UpdateLedgerAccountTypeValidator()
		{
			RuleFor(x => x.Id)
				.NotEmpty().WithMessage("ID loại tài khoản không được để trống.");
			RuleFor(x => x.Code)
				.NotEmpty().WithMessage("Mã loại tài khoản không được để trống.")
				.MaximumLength(64).WithMessage("Mã loại tài khoản không được vượt quá 64 ký tự.");
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Tên loại tài khoản không được để trống.")
				.MaximumLength(250).WithMessage("Tên loại tài khoản không được vượt quá 250 ký tự.");
			RuleFor(x => x.Kind)
				.IsInEnum().WithMessage("Loại tài khoản không hợp lệ.");
			RuleFor(x => x.Description)
				.MaximumLength(500).WithMessage("Mô tả không được vượt quá 500 ký tự.");
		}
	}
}
