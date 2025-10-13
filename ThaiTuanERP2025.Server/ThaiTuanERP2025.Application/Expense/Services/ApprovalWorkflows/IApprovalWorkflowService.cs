using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Services.ApprovalWorkflows
{
	public sealed record StepOverrideRequest(int StepOrder, Guid? SelectedApproverId);
	public interface IApprovalWorkflowService
	{
		Task<Guid> CreateInstanceForExpensePaymentAsync(ExpensePayment expensePayment, Guid workflowTemplateId, IReadOnlyCollection<StepOverrideRequest>? overrides, bool linkToPayment, CancellationToken cancellationToken);
	}
}
