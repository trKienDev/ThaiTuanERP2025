using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Validators
{
	public class AddUserToGroupDtoValidator : AbstractValidator<AddUserToGroupDto>
	{
		public AddUserToGroupDtoValidator() {
			RuleFor(x => x.UserId)
				.NotEmpty().WithMessage("UserId không được để trống.");
		}
	}
}
