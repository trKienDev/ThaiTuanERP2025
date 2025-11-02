using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Alerts.Notifications
{
	public interface INotificationService
	{
		Task NotifyStepActivatedAsync(ExpenseWorkflowInstance instance, ExpenseStepInstance step, IReadOnlyCollection<Guid> targetUserIds, CancellationToken cancellationToken);
		Task NotifyWorkflowRejectedAsync(ExpenseWorkflowInstance workflow, ExpenseStepInstance step, string docName, Guid docId, string docType, IReadOnlyCollection<Guid> targetUserIds, CancellationToken cancellationToken);
		Task NotifyWorkflowApprovedAsync(ExpenseWorkflowInstance workflow, ExpenseStepInstance step, IReadOnlyCollection<Guid> targetUserIds, string approver, string docName, Guid documentId, string documentType, CancellationToken cancellationToken = default);
	}
}
