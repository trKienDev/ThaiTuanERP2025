using MediatR;

namespace ThaiTuanERP2025.Application.Alerts.TaskReminders.Queries.GetMyReminders
{
	public sealed record GetMyRemindersQuery : IRequest<IReadOnlyCollection<TaskReminderDto>>;
}
