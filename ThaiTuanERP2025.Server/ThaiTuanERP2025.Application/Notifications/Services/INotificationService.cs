using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Notifications.Services
{
	public interface INotificationService
	{
		Task NotifyStepActivatedAsync(ApprovalWorkflowInstance instance, ApprovalStepInstance step, IReadOnlyCollection<Guid> targetUserIds, CancellationToken cancellationToken);
		Task NotifyWorkflowRejectedAsync(ApprovalWorkflowInstance workflow, ApprovalStepInstance step, string docName, Guid docId, string docType, IReadOnlyCollection<Guid> targetUserIds, CancellationToken cancellationToken);
		Task NotifyWorkflowApprovedAsync(ApprovalWorkflowInstance workflow, ApprovalStepInstance step, IReadOnlyCollection<Guid> targetUserIds, string approver, string docName, Guid documentId, string documentType, CancellationToken cancellationToken = default);
	}
}
