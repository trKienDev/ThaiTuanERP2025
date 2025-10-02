using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.Commands.BudgetGroup.UpdateBudgetGroup;

namespace ThaiTuanERP2025.Application.Account.Commands.Groups.UpdateGroup
{
	public class UpdateGroupCommandValidator : AbstractValidator<UpdateGroupCommand>
	{
		public UpdateGroupCommandValidator() {
			RuleFor(x => x.GroupId)
				.NotEmpty().WithMessage("GroupId không được để trống.");

			RuleFor(x => x.NewName)
				.NotNull().WithMessage("Tên nhóm không được để trống.")
				.MaximumLength(100).WithMessage("Tên nhóm không được vượt quá 100 ký tự.");

			RuleFor(x => x.NewDescription)
				.NotNull().WithMessage("Mô tả nhóm không được để trống.")
				.MaximumLength(500).WithMessage("Mô tả nhóm không được vượt quá 500 ký tự.");

			RuleFor(x => x.RequestingUserId)
				.NotEmpty().WithMessage("RequestingUserId không được để trống.");
		}
	}
}
