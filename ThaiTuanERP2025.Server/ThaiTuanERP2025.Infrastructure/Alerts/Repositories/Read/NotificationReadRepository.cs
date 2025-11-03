using AutoMapper;
using ThaiTuanERP2025.Application.Alerts.Notifications;
using ThaiTuanERP2025.Domain.Alerts.Entities;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Alerts.Repositories.Read
{
	public sealed class NotificationReadRepository : BaseReadRepository<AppNotification, AppNotificationDto>, INotificationReadRepository
	{
		private ThaiTuanERP2025DbContext DbContext => (ThaiTuanERP2025DbContext)_dbContext;
		public NotificationReadRepository(ThaiTuanERP2025DbContext context, IMapper mapper)
			: base(context, mapper) { }
	}
}
