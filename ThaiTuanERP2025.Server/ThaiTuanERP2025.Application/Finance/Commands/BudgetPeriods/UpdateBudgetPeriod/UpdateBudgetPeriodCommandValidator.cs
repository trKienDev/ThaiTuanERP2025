using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetPeriods.UpdateBudgetPeriod
{
	public class UpdateBudgetPeriodCommandValidator : AbstractValidator<UpdateBudgetPeriodCommand>
	{
		public UpdateBudgetPeriodCommandValidator() {
			RuleFor(x => x.Id)
				.NotEmpty().WithMessage("Id không được để trống")
				.NotNull().WithMessage("Id không được để trống");

			RuleFor(x => x.IsActice)
				.NotNull().WithMessage("Trạng thái hoạt động không được để trống");
		}
	}
}
