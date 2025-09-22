using FluentValidation;
using ThaiTuanERP2025.Application.Expense.Request;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.OverrideApprover
{
	public sealed class OverrideApproverValidator : AbstractValidator<OverrideApproverRequest>
	{
		public OverrideApproverValidator() {
			RuleFor(x => x.NewApproverId).NotEmpty();
			RuleFor(x => x.Reason).MaximumLength(256).WithMessage("Lý do không vượt quá 256 ký tự");
		}
	}
}
