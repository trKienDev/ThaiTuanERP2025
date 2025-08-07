using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.Commands.BudgetPeridos.CreateBudgetPeriod;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetPeriods.CreateBudgetPeriod
{
	public class CreateBudgetPeriodCommandValidator : AbstractValidator<CreateBudgetPeriodCommand>
	{
		public CreateBudgetPeriodCommandValidator() {
			RuleFor(x => x.Year)
				.InclusiveBetween(2025, DateTime.UtcNow.Year)
				.WithMessage("Năm không hợp lệ");

			RuleFor(x => x.Month)
				.InclusiveBetween(1, 12)
				.WithMessage("Tháng không hợp lệ");
		}
	}
}
