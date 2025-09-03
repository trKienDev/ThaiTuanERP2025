using FluentValidation;

namespace ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflows.GetApprovalWorkflowById
{
	public sealed class GetApprovalWorkflowByIdValidator : AbstractValidator<GetApprovalWorkflowByIdQuery>
	{
		public GetApprovalWorkflowByIdValidator() {
			RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
		}
	}
}
