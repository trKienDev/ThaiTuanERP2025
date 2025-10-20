using MediatR;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.ApproveCurrentStep
{
	public sealed record ApproveCurrentStepCommand(
		Guid WorkflowInstanceId,
		Guid StepInstanceId,
		Guid UserId,
		Guid PaymentId,
		string? Comment // optional, để lưu lịch sử nếu bạn muốn
	) : IRequest<Unit>;
}
