namespace ThaiTuanERP2025.Application.Core.Reminders.Contracts
{
	public sealed record ReminderPayload (
		Guid UserId,
		Guid ReminderId,
		string Subject,
		string Message,
		string LinkUrl,
		int SlaHours,
		DateTime DueAt
	);
}
