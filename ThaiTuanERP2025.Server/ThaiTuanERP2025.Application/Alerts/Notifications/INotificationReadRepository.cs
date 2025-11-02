using ThaiTuanERP2025.Application.Common.Repositories;
using ThaiTuanERP2025.Domain.Alerts.Entities;

namespace ThaiTuanERP2025.Application.Alerts.Notifications
{
	public interface INotificationReadRepository : IBaseReadRepository<AppNotification, AppNotificationDto>
	{
	}
}
