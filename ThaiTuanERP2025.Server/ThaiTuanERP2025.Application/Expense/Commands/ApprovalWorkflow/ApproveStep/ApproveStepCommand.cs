using MediatR;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflow.ApproveStep
{
	public sealed record ApproveStepCommand(
		Guid StepInstanceId, 
		string? Comment,
		byte[] RowVersion // optimistic concurrency của StepInstance
	) : IRequest<Unit>;
}
