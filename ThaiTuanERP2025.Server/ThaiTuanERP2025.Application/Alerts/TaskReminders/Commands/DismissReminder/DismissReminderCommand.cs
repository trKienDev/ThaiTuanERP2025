using MediatR;

namespace ThaiTuanERP2025.Application.Alerts.TaskReminders.Commands.DismissReminder
{
	public sealed record DismissReminderCommand(Guid Id) : IRequest<Unit>;
}
