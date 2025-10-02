using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.Commands.CashoutGroups.CashoutOutGroup;

namespace ThaiTuanERP2025.Application.Finance.Commands.CashoutGroups.UpdateCashoutGroup
{
	public class UpdateCashoutGroupValidator : AbstractValidator<UpdateCashoutGroupCommand>
	{
		public UpdateCashoutGroupValidator() {
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Tên là bắt buộc")
				.MaximumLength(200).WithMessage("Tối đa 200 ký tự");
		}
	}
}
