using MediatR;
using ThaiTuanERP2025.Application.Notifications.Dtos;

namespace ThaiTuanERP2025.Application.Notifications.Queries.TaskReminders.GetMyReminders
{
	public sealed record GetMyRemindersQuery : IRequest<IReadOnlyCollection<TaskReminderDto>>;
}
