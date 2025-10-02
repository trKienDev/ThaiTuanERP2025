using FluentValidation;

namespace ThaiTuanERP2025.Application.Account.Commands.Users.CreateUser
{
	public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
	{
		public CreateUserCommandValidator()
		{
			RuleFor(x => x.FullName)
				.NotEmpty().WithMessage("Tên không được để trống")
				.MaximumLength(100);
			RuleFor(x => x.Username)
				.NotEmpty().WithMessage("Username không được để trống")
				.MinimumLength(4).WithMessage("Username tối thiểu 4 ký tự");
			RuleFor(x => x.EmployeeCode)
				.NotEmpty().WithMessage("Mã nhân viên không được để trống")
				.MinimumLength(4).WithMessage("Username tối thiểu 4 ký tự");
			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Mật khẩu không được để trống")
				.MinimumLength(6).WithMessage("Mật khẩu tối đa 6 ký tự");
			RuleFor(x => x.Role)
				.NotEmpty().WithMessage("Vai trò không được để trống");
			RuleFor(x => x.Position)
				.NotEmpty().WithMessage("Chức vụ không được để trống");
			RuleFor(x => x.DepartmentId)
				.NotEmpty().WithMessage("Phòng ban không được để trống");
			RuleFor(x => x.Email)
				.EmailAddress().WithMessage("Email không hợp lệ")
				.When(x => !string.IsNullOrWhiteSpace(x.Email));
			RuleFor(x => x.Phone)
				.Matches(@"^[0-9]{9,11}$").WithMessage("Số điện thoại không hợp lệ.")
				.When(x => !string.IsNullOrWhiteSpace(x.Phone));
		}
	}
}
