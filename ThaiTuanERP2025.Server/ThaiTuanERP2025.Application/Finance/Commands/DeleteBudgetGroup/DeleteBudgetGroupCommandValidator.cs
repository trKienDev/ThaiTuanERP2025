using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Commands.DeleteBudgetGroup
{
	public class DeleteBudgetGroupCommandValidator : AbstractValidator<DeleteBudgetGroupCommand>
	{
		public DeleteBudgetGroupCommandValidator() {
			RuleFor(x => x.Id)
				.NotEmpty().WithMessage("Id không hợp lệ.")
				.NotNull().WithMessage("Id không hợp lệ");	
		}	
	}
}
