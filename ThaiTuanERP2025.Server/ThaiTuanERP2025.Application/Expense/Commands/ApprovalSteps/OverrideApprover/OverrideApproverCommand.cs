using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Request;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.OverrideApprover
{
	public sealed record OverrideApproverCommand(
		Guid WorkflowId,
		Guid StepId, 
		OverrideApproverRequest Body
	) : IRequest<ApprovalStepInstanceDto>;
}
