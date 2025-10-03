using MediatR;

namespace ThaiTuanERP2025.Application.Notifications.Queries.Notifications.GetUnreadCount
{
	public sealed record GetUnreadCountQuery() : IRequest<int>;
}
