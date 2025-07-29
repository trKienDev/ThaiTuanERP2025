using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Account.Commands.ChangePassword
{
	public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
	{
		public ChangePasswordCommandValidator() {
			RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Vui lòng nhập mật khẩu hiện tại");
			RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6).WithMessage("Mật khẩu mới tối thiểu 6 ký tự");
			RuleFor(x => x.ConfirmPassword).Equal(x => x.NewPassword).WithMessage("Xác nhận mật khẩu không khớp");
		}
	}
}
