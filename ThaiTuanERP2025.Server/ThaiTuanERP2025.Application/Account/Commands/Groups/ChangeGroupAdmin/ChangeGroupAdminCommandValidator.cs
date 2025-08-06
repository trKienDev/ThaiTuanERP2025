using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Commands.Group.ChangeGroupAdmin;

namespace ThaiTuanERP2025.Application.Account.Commands.Groups.ChangeGroupAdmin
{
	public class ChangeGroupAdminCommandValidator : AbstractValidator<ChangeGroupAdminCommand> {
		public ChangeGroupAdminCommandValidator()
		{
			RuleFor(x => x.GroupId)
				.NotEmpty().WithMessage("GroupId không được để trống.");

			RuleFor(x => x.TargetUserId)
				.NotEmpty().WithMessage("TargetUserId không được để trống.");

			RuleFor(x => x.RequestorId)
				.NotEmpty().WithMessage("RequestorId không được để trống.");
		}
	}
}
