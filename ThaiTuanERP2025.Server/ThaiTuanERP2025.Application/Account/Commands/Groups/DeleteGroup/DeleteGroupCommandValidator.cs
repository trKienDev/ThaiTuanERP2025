using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Commands.Group.DeleteGroup;

namespace ThaiTuanERP2025.Application.Account.Commands.Groups.DeleteGroup
{
	public class DeleteGroupCommandValidator : AbstractValidator<DeleteGroupCommand>
	{	
		public DeleteGroupCommandValidator()
		{
			RuleFor(x => x.GroupId)
				.NotEmpty().WithMessage("GroupId không được để trống.");
			RuleFor(x => x.RequestingUserId)
				.NotEmpty().WithMessage("RequestingUserId không được để trống.");
		}
	}
}
