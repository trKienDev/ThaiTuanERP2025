using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Request;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.ApproveStep
{
	public sealed record ApproveStepCommand(
		Guid WorkflowId, 
		Guid StepId, 
		ApproveStepRequest Body
	) : IRequest<ApprovalStepInstanceDto>;
}
