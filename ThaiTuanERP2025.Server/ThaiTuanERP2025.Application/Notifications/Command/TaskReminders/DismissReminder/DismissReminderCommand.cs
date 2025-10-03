using MediatR;

namespace ThaiTuanERP2025.Application.Notifications.Command.TaskReminders.DismissReminder
{
	public sealed record DismissReminderCommand(Guid Id) : IRequest<Unit>;
}
