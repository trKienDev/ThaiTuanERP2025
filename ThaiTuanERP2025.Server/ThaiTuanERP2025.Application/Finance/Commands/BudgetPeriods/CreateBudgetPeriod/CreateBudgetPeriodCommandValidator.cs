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
				.Must(year => year == DateTime.UtcNow.Year)
				.WithMessage("Năm không hợp lệ");

			RuleFor(x => x.Month)
				.Must((command, month) =>
				{
					Console.WriteLine($"[DEBUG] Year: {command.Year}, Month: {month}");

					var currentMonth = DateTime.UtcNow.Month;
					var nextMonth = currentMonth == 12 ? 1 : currentMonth + 1;
					return month == currentMonth || month == nextMonth;
				})
				.WithMessage("Chỉ được tạo kỳ ngân sách cho tháng hiện tại hoặc tháng kế tiếp");

		}
	}
}
