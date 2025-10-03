using MediatR;

namespace ThaiTuanERP2025.Application.Notifications.Queries.GetUnreadCount
{
	public sealed record GetUnreadCountQuery() : IRequest<int>;
}
