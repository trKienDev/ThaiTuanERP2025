using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetCodes.UpdateBudgetCodeStatus
{
	public class UpdateBudgetCodeStatusCommandValidator : AbstractValidator<UpdateBudgetCodeStatusCommand>
	{
		public UpdateBudgetCodeStatusCommandValidator()
		{
			RuleFor(x => x.Id)
				.NotEmpty().WithMessage("Mã ngân sách không được để trống");
			RuleFor(x => x.IsActive)
				.NotNull().WithMessage("Trạng thái không được để trống");
		}
	}
}
