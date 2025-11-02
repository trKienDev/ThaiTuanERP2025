using MediatR;

namespace ThaiTuanERP2025.Application.Alerts.Notifications.Queries.GetUnreadCount
{
	public sealed record GetUnreadCountQuery() : IRequest<int>;
}
