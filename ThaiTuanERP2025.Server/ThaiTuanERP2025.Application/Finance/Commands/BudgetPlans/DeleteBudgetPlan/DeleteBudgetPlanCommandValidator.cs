using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetPlans.DeleteBudgetPlan
{
	public class DeleteBudgetPlanCommandValidator : AbstractValidator<DeleteBudgetPlanCommand>
	{
		public DeleteBudgetPlanCommandValidator()
		{
			RuleFor(x => x.Id)
				.NotEmpty().WithMessage("Id Kế hoạch ngân sách không được để trống")
				.NotEqual(Guid.Empty).WithMessage("Id Kế hoạch ngân sách không hợp lệ");
		}
	}
}
