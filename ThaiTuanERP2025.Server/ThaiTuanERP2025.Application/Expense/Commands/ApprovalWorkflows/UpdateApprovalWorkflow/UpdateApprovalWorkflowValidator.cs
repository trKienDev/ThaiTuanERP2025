using FluentValidation;
using ThaiTuanERP2025.Application.Expense.Requests;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflows.UpdateApprovalWorkflow
{
	public sealed class UpdateApprovalWorkflowValidator : AbstractValidator<UpdateApprovalWorkflowRequest>
	{
		public UpdateApprovalWorkflowValidator() {
			RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
			RuleFor(x => x.Steps).NotNull().NotEmpty();

			RuleForEach(x => x.Steps).ChildRules(steps =>
			{
				steps.RuleFor(s => s.Title).NotEmpty().MaximumLength(256);
				steps.RuleFor(s => s.Order).GreaterThan(0);
				steps.RuleFor(s => s.CandidateUserIds).NotNull();
			});

			// optional: đảm bảo Order là duy nhất
			RuleFor(x => x.Steps.Select(s => s.Order))
				.Must(o => o.Distinct().Count() == o.Count())
				.WithMessage("Step orders must be unique");
		}
	}
}
