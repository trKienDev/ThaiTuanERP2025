using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Validators
{
	public class CreateGroupDtoValidator : AbstractValidator<CreateGroupDto>
	{
		public CreateGroupDtoValidator() {
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Tên nhóm không được để trống.")
				.MaximumLength(100).WithMessage("Tên nhóm không được vượt quá 100 ký tự.");
			RuleFor(x => x.Description)
				.MaximumLength(500).WithMessage("Mô tả nhóm không được vượt quá 500 ký tự.");
			RuleFor(x => x.AdminUserId)
				.NotEmpty().WithMessage("AdminUserId không được để trống.")
				.Must(id => id != Guid.Empty).WithMessage("AdminUserId không được là Guid.Empty.");
		}
	}
}
