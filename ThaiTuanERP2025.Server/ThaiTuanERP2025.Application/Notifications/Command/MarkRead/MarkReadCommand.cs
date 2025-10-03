using MediatR;

namespace ThaiTuanERP2025.Application.Notifications.Command.MarkRead
{
	public sealed record MarkReadCommand(Guid Id) : IRequest<Unit>;
}
