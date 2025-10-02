using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Notifications.Services
{
	public interface INotificationService
	{
		Task NotifyStepActivatedAsync(ApprovalWorkflowInstance instance, ApprovalStepInstance step, IReadOnlyCollection<Guid> targetUserIds, CancellationToken cancellationToken);
	}
}
