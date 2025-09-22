using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Request;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.ReopenStep
{
	public sealed record ReopenStepCommand (
		Guid WorkflowId,
		Guid StepId,
		ReopenStepRequest Body
	) : IRequest<ApprovalStepInstanceDto>;
}
