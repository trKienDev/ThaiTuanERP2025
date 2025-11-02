using MediatR;

namespace ThaiTuanERP2025.Application.Alerts.Notifications.Commands.MarkRead
{
	public sealed record MarkReadCommand(Guid Id) : IRequest<Unit>;
}
