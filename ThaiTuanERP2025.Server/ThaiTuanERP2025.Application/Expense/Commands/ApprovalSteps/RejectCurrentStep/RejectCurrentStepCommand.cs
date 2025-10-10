using MediatR;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.RejectCurrentStep
{
	public sealed record RejectCurrentStepCommand(
		Guid WorkflowInstanceId,
		Guid StepInstanceId,
		Guid UserId,
		Guid PaymentId,
		string? Comment 
	) : IRequest<Unit>;
}
