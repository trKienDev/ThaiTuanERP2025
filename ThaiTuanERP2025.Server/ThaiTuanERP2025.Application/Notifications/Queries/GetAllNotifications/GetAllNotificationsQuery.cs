using MediatR;
using ThaiTuanERP2025.Application.Notifications.Dtos;

namespace ThaiTuanERP2025.Application.Notifications.Queries.GetAllNotifications
{
	public sealed record GetAllNotificationsQuery
	(
		bool UnreadOnly = false,
		int Page = 1,
		int PageSize = 20
	) : IRequest<IReadOnlyCollection<AppNotificationDto>>;
}
