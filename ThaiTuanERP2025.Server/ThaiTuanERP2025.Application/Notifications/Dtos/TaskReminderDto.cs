namespace ThaiTuanERP2025.Application.Notifications.Dtos
{
	public sealed record TaskReminderDto(
		Guid Id,
		string Title,
		string Message,
		DateTimeOffset DueAt,
		Guid WorkflowInstanceId,
		Guid StepInstanceId,
		Guid DocumentId,
		string DocumentType
	);
}
