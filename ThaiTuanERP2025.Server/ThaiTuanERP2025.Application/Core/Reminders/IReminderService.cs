using ThaiTuanERP2025.Domain.Core.Enums;

namespace ThaiTuanERP2025.Application.Core.Reminders
{
	public interface IReminderService
	{
		Task ScheduleReminderAsync(Guid userId, string subject, string message, int slaHours, DateTime dueAt, LinkType linkType, Guid targetId, CancellationToken cancellationToken = default);
	}
}
