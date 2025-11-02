using MediatR;

namespace ThaiTuanERP2025.Application.Alerts.Notifications.Queries.GetAllNotifications
{
	public sealed record GetAllNotificationsQuery
	(
		bool UnreadOnly = false,
		int Page = 1,
		int PageSize = 20
	) : IRequest<IReadOnlyCollection<AppNotificationDto>>;
}
