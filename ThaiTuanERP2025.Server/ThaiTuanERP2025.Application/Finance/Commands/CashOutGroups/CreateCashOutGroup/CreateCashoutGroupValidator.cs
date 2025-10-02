using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Commands.CashoutGroups.CreateCashoutGroup
{
	public class CreateCashoutGroupValidator : AbstractValidator<CreateCashoutGroupCommand>
	{
		public CreateCashoutGroupValidator() {
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Tên là bắt buộc")
				.MaximumLength(200).WithMessage("Tối đa 200 ký tự");
		}
	}
}
