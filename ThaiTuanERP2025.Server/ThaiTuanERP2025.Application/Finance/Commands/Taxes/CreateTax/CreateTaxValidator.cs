using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Application.Finance.Commands.Taxes.CreateTax
{
	public class CreateTaxValidator : AbstractValidator<CreateTaxCommand>
	{
		public CreateTaxValidator() {
			RuleFor(x => x.PolicyName).NotEmpty().WithMessage("Tên chính sách thuế không được để trống")
				.MaximumLength(200).WithMessage("Tên chính sách thuế không vượt quá 200 ký tự");
			RuleFor(x => x.Rate).InclusiveBetween(0, 100).WithMessage("% phải từ 0 - 100%");
			RuleFor(x => x.TaxBroadType).IsInEnum();

			When(x => x.TaxBroadType == Domain.Finance.Enums.TaxBroadType.Consumption, () =>
			{
				RuleFor(x => x.ConsumptionSubType).NotNull();
			});

			// Nếu KHÔNG phải Consumption, ConsumptionSubType phải null
			When(x => x.TaxBroadType != Domain.Finance.Enums.TaxBroadType.Consumption, () =>
			{
				RuleFor(x => x.ConsumptionSubType).Equal((ConsumptionSubType?)null);
			});
		}
	}
}
