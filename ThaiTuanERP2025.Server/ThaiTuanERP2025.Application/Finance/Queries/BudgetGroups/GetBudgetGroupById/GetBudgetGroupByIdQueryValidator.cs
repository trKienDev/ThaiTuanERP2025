using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Queries.BudgetGroups.GetBudgetGroupById
{
	public class GetBudgetGroupByIdQueryValidator : AbstractValidator<GetBudgetGroupByIdQuery>
	{
		public GetBudgetGroupByIdQueryValidator() {
			RuleFor(x => x.Id)
				.NotEmpty().WithMessage("Id không được để trống.")
				.NotNull().WithMessage("Id không được null.")
				.Must(id => id != Guid.Empty).WithMessage("Id nhóm ngân sách không hợp lệ");	
		}
	}
}
