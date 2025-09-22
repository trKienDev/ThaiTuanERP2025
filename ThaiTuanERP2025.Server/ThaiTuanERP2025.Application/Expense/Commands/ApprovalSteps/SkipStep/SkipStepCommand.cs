using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.SkipStep
{
	public sealed record SkipStepCommand(
		Guid WorkflowId, 
		Guid StepId,
		string Reason
	) : IRequest<ApprovalStepInstanceDto>;
}
