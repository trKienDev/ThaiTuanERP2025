using MediatR;

namespace ThaiTuanERP2025.Application.Alerts.Notifications.Commands.MarkAllRead
{
	public sealed record MarkAllReadCommand() : IRequest<Unit>;
}
