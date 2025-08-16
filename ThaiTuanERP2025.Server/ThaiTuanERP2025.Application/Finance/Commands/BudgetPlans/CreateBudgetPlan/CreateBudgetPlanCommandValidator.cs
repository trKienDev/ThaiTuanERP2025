using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetPlans.CreateBudgetPlan
{
	public class CreateBudgetPlanCommandValidator : AbstractValidator<CreateBudgetPlanCommand>
	{
		public CreateBudgetPlanCommandValidator()
		{
			RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("DepartmentId không được để trống");
			RuleFor(x => x.BudgetCodeId).NotEmpty().WithMessage("BudgetCodeId không được để trống");
			RuleFor(x => x.BudgetPeriodId).NotEmpty().WithMessage("BudgetPeriodId không được để trống");
			RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount phải lớn hơn 0");
		}
	}
}
