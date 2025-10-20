using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Services.ApprovalWorkflows
{
	public interface IApprovalStepService
	{
		Task PublishAsync(ApprovalWorkflowInstance workflowInstance, ApprovalStepInstance stepInstance, string docName, Guid docId, string docType, CancellationToken cancellationToken);
	}
}
