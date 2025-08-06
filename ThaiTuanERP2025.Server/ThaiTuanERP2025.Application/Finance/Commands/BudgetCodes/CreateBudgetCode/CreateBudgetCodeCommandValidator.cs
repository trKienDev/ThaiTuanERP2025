using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetCodes.CreateBudgetCode
{
	public class CreateBudgetCodeCommandValidator : AbstractValidator<CreateBudgetCodeCommand>
	{
		public CreateBudgetCodeCommandValidator()
		{
			RuleFor(x => x.Code).MaximumLength(20);
			RuleFor(x => x.Code).NotEmpty().WithMessage("Mã ngân sách không được để trống.");

			RuleFor(x => x.Name).MaximumLength(100);
			RuleFor(x => x.Name).NotEmpty().WithMessage("Tên ngân sách không được để trống.");

			RuleFor(x => x.BudgetGroupId).NotEmpty().WithMessage("Phải chọn nhóm ngân sách.");
		}
	}
}
