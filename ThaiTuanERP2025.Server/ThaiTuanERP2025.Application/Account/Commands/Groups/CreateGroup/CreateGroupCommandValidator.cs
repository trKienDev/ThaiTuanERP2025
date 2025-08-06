using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Commands.Group.CreateGroup;

namespace ThaiTuanERP2025.Application.Account.Commands.Groups.CreateGroup
{
	public class CreateGroupCommandValidator : AbstractValidator<CreateGroupCommand>
	{
		public CreateGroupCommandValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Tên nhóm không được để trống.")
				.MaximumLength(100).WithMessage("Tên nhóm không được vượt quá 100 ký tự.");
			RuleFor(x => x.Description)
				.MaximumLength(500).WithMessage("Mô tả không được vượt quá 500 ký tự.");
			RuleFor(x => x.AdminUserId)
				.NotEmpty().WithMessage("AdminId không được để trống.");
		}
	}
}
