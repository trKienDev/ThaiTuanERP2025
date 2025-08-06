using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Validators;

namespace ThaiTuanERP2025.Application.Account.Commands.Groups.AddUserToGroup
{
	public class AddUserToGroupCommandValidator : AbstractValidator<AddUserToGroupDto>
	{
		public AddUserToGroupCommandValidator()
		{
			RuleFor(x => x.UserId)
				.NotEmpty().WithMessage("UserId không được để trống.");
		}
	}
}
