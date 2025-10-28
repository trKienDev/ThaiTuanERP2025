using FluentValidation;
using ThaiTuanERP2025.Application.Account.Commands.Login;

namespace ThaiTuanERP2025.Application.Account.Commands.Accounts.Login
{
	public class LoginCommandValidator : AbstractValidator<LoginCommand>
	{
		public LoginCommandValidator() {
			RuleFor(x => x.EmployeeCode)
				.NotEmpty().WithMessage("Mã nhân viên không được để trống");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Mật khẩu không được để trống");
		}
	}
}
