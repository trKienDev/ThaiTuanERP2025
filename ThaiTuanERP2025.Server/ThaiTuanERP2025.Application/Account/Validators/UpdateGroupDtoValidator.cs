using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Validators
{
	public class UpdateGroupDtoValidator : AbstractValidator<UpdateGroupDto>
	{
		public UpdateGroupDtoValidator() {
			RuleFor(x => x.Name).NotEmpty().WithMessage("Tên nhóm không được để trống.")
				.MaximumLength(100).WithMessage("Tên nhóm không được vượt quá 100 ký tự.");
			RuleFor(x => x.Description).MaximumLength(500).WithMessage("Mô tả nhóm không được vượt quá 500 ký tự");
			RuleFor(x => x.RequestorId).NotEmpty().WithMessage("RequestorId không được để trống.");
		}
	}
}
