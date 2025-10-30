using FluentValidation;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetCodes.ToggleBudgetCodeActive
{
	public class ToggleBudgetCodeActiveValidator : AbstractValidator<ToggleBudgetCodeActiveCommand>
	{
		public ToggleBudgetCodeActiveValidator()
		{
			RuleFor(x => x.Id)
				.NotEmpty().WithMessage("Mã ngân sách không được để trống");
		}
	}
}
