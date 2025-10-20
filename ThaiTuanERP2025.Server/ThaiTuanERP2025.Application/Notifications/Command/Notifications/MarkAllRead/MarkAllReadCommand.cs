using MediatR;

namespace ThaiTuanERP2025.Application.Notifications.Command.Notifications.MarkAllRead
{
	public sealed record MarkAllReadCommand() : IRequest<Unit>;
}
