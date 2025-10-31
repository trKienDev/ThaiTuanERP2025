using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Services.ApprovalWorkflows
{
	public interface IApprovalStepService
	{
		Task PublishAsync(ExpenseWorkflowInstance workflowInstance, ExpenseStepInstance stepInstance, string docName, Guid docId, string docType, CancellationToken cancellationToken);
	}
}
