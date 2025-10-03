using MediatR;

namespace ThaiTuanERP2025.Application.Notifications.Command.MarkAllRead
{
	public sealed record MarkAllReadCommand() : IRequest<Unit>;
}
