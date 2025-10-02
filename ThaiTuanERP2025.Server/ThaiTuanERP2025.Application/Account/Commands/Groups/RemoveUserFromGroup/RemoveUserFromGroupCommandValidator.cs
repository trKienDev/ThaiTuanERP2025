using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Commands.Group.RemoveUserFromGroup;

namespace ThaiTuanERP2025.Application.Account.Commands.Groups.RemoveUserFromGroup
{
	public class RemoveUserFromGroupCommandValidator : AbstractValidator<RemoveUserFromGroupCommand>
	{
		public RemoveUserFromGroupCommandValidator() { 
			RuleFor(x => x.GroupId)
				.NotEmpty().WithMessage("GroupId không được để trống.");
			RuleFor(x => x.TargetUserId)
				.NotEmpty().WithMessage("TargetUserId không được để trống.");
			RuleFor(x => x.RequestingUserId)
				.NotEmpty().WithMessage("RequestingUserId không được để trống.");
		}
	}
}
