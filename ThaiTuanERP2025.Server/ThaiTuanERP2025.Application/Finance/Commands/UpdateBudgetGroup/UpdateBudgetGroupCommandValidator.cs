using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Commands.UpdateBudgetGroup
{
	public class UpdateBudgetGroupCommandValidator : AbstractValidator<UpdateBudgetGroupCommand>
	{
		public UpdateBudgetGroupCommandValidator() {
			RuleFor(x => x.Id)
				.NotEmpty().WithMessage("Id không hợp lệ.")
				.NotNull().WithMessage("Id không hợp lệ");

			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Tên nhóm ngân sách không được để trống.")
				.MaximumLength(100).WithMessage("Tên nhóm ngân sách không được vượt quá 100 ký tự.");

			RuleFor(x => x.Code)
				.NotEmpty().WithMessage("Mã nhóm ngân sách không được để trống.")
				.MaximumLength(20).WithMessage("Mã nhóm ngân sách không được vượt quá 20 ký tự.")
				.Matches(@"^[A-Z0-9]+$").WithMessage("Mã nhóm ngân sách chỉ được chứa chữ hoa và số, không có dấu cách hoặc ký tự đặc biệt.");
		}
	}
}
