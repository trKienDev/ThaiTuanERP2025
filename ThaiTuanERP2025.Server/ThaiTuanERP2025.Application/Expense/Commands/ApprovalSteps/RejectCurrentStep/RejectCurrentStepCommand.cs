using MediatR;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.RejectCurrentStep
{
	public sealed record RejectCurrentStepCommand(
		Guid WorkflowInstanceId,
		Guid StepInstanceId,
		Guid UserId,
		string? Reason // có thể bắt buộc nếu policy yêu cầu
	) : IRequest<Unit>;
}
