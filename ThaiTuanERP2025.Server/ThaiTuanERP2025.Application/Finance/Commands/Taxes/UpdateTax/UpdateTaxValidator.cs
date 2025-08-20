using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Application.Finance.Commands.Taxes.UpdateTax
{
	public class UpdateTaxValidator : AbstractValidator<UpdateTaxCommand>
	{
		public UpdateTaxValidator() {
			RuleFor(x => x.Id).NotEmpty().WithMessage("Id không được để trống");
			RuleFor(x => x.PolicyName).NotEmpty().WithMessage("Chính sách thuế không được đẻ trống")
				.MaximumLength(200).WithMessage("Độ dài tối đa 200 ký tự");
			RuleFor(x => x.Rate).InclusiveBetween(0, 100).WithMessage("Tỷ là từ 0 - 100%");

			RuleFor(x => x.PostingLedgerAccountId).NotEmpty();
		}
	}
}
