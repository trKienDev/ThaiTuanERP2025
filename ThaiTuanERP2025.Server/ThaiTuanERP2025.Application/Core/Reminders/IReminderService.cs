namespace ThaiTuanERP2025.Application.Core.Reminders
{
	public interface IReminderService
	{
		Task ScheduleReminderAsync(Guid userId, string subject, string message, int slaHours, DateTime dueAt, string? linkUrl = null, CancellationToken cancellationToken = default);
	}
}
