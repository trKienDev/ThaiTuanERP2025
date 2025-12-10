using ThaiTuanERP2025.Application.Core.Notifications.Contracts;
using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Application.Core.Notifications
{
	public interface IUserNotificationReadRepository : IBaseReadRepository<UserNotification, UserNotificationDto>
	{
	}
}
