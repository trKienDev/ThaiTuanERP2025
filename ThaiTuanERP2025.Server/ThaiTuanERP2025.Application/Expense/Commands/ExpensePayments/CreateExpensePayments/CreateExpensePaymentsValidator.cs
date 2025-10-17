using FluentValidation;

namespace ThaiTuanERP2025.Application.Expense.Commands.ExpensePayments.CreateExpensePayments
{
	public sealed class CreateExpensePaymentsValidator : AbstractValidator<CreateExpensePaymentCommand>
	{
		public CreateExpensePaymentsValidator() {
			RuleFor(x => x.Request.Name).NotEmpty().MaximumLength(256);
			RuleFor(x => x.Request.DueDate).NotEmpty();
			RuleForEach(x => x.Request.Items).ChildRules(item =>
			{
				item.RuleFor(i => i.ItemName).NotEmpty().MaximumLength(256);
				item.RuleFor(i => i.Quantity).GreaterThanOrEqualTo(1);
				item.RuleFor(i => i.UnitPrice).GreaterThanOrEqualTo(0);
				item.RuleFor(i => i.TaxRate).InclusiveBetween(0, 1);
			});
		}
	}
}
