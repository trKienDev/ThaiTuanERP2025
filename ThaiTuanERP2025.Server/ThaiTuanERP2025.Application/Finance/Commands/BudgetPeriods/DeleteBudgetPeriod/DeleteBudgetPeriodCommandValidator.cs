using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetPeriods.DeleteBudgetPeriod
{
	public class DeleteBudgetPeriodCommandValidator : AbstractValidator<DeleteBudgetPeriodCommand>
	{
		public DeleteBudgetPeriodCommandValidator()
		{
			RuleFor(x => x.Id)
				.NotEmpty().WithMessage("Id không được để trống")
				.NotNull().WithMessage("Id không được để trống");
		}
	}
}
