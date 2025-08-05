using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Commands.CreateBudgetGroup
{
	public class CreateBudgetGroupCommandValidator : AbstractValidator<CreateBudgetGroupCommand>
	{
		public CreateBudgetGroupCommandValidator()
		{
			RuleFor(x => x.Code)
				.NotEmpty().WithMessage("Mã nhóm ngân sách không được để trống.")
				.MaximumLength(20).WithMessage("Mã nhóm ngân sách không được vượt quá 20 ký tự.")
				.Matches(@"^[A-Z0-9]+$").WithMessage("Mã nhóm ngân sách chỉ được chứa chữ hoa và số, không có dấu cách hoặc ký tự đặc biệt.");

			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Tên nhóm ngân sách không được để trống.")
				.MaximumLength(100).WithMessage("Tên nhóm ngân sách không được vượt quá 100 ký tự.");

		}
	}
}
