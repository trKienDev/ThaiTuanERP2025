using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Request;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.RejectStep
{
	public sealed record RejectStepCommand(
		Guid WorkflowId, 
		Guid StepId, 
		RejectStepRequest Body
	) : IRequest<ApprovalStepInstanceDto>;
}
