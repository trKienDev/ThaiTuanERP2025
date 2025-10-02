using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Validators
{
	public class RemoveUserDtoValidator : AbstractValidator<RemoveUserDto>
	{
		public RemoveUserDtoValidator() {
			RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId không được để trống.");
			RuleFor(x => x.RequestorId).NotEmpty().WithMessage("RequestorId không được để trống.");
		}
	}
}
