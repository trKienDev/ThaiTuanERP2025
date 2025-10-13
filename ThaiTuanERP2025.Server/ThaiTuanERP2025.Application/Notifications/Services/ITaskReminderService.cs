namespace ThaiTuanERP2025.Application.Notifications.Services
{
	public interface ITaskReminderService
	{
		Task CreateForStepActivationAsync(Guid stepInstanceId, Guid workflowInstanceId, IEnumerable<Guid> userIds, string title, string message, Guid documentId, string documentType, DateTime dueAt, CancellationToken cancellationToken);
		Task ResolveByStepAsync(Guid stepInstanceId, string reason, CancellationToken cancellationToken);
		Task ResolveOneAsync(Guid reminderId, string reason, CancellationToken cancellationToken);
		Task ResolveByWorkflowAsync(Guid workflowInstanceId, string reason, CancellationToken cancellationToken);
	}
}
