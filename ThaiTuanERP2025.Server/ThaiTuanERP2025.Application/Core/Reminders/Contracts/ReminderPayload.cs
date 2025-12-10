namespace ThaiTuanERP2025.Application.Core.Reminders.Contracts
{
	public sealed record ReminderCreatedPayload (
		Guid UserId,
		Guid ReminderId,
		string Subject,
		string Message,
		string LinkUrl,
		int SlaHours,
		DateTime DueAt
	);

	public record ReminderResolvedPayload(
		Guid ReminderId,
		Guid UserId,
		string Subject,
		DateTime ResolvedAt
	);
}
