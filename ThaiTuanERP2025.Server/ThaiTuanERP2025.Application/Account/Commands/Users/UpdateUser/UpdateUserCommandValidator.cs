using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Account.Commands.Users.UpdateUser
{
	public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
	{
		public UpdateUserCommandValidator()
		{
			RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
			RuleFor(x => x.Position).NotEmpty().MaximumLength(50);
			RuleFor(x => x.Role).NotEmpty();
			RuleFor(x => x.DepartmentId).NotEmpty();

			RuleFor(x => x.Email).EmailAddress().WithMessage("Email không hợp lệ")
				.When(x => !string.IsNullOrWhiteSpace(x.Email));
			RuleFor(x => x.Phone).Matches(@"^[0-9]{9,11}$").WithMessage("Số điện thoại không hợp lệ")
				.When(x => !string.IsNullOrWhiteSpace(x.Phone));
		}
	}
}
