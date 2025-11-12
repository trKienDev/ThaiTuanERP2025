using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Core.Notifications;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Core.Enums;

namespace ThaiTuanERP2025.Application.Core.Services
{
	public class NotificationService : INotificationService
	{
		private readonly IUnitOfWork _uow;
		private readonly IRealtimeNotifier _realtime;
		public NotificationService(IUnitOfWork uow, IRealtimeNotifier realtime) {
			_uow = uow;	
			_realtime = realtime;
		}

		public async Task SendAsync(Guid userId, string title, string message, string? linkUrl = null, NotificationType type = NotificationType.Info, CancellationToken cancellationToken = default)
		{
			var entity = new UserNotification(userId, title, message, linkUrl, type);
			await _uow.UserNotifications.AddAsync(entity, cancellationToken);
			await _uow.SaveChangesWithoutDispatchAsync(cancellationToken);

			// push realtime
			var payload = new UserNotificationDto {
				Id = entity.Id,
				Title = entity.Title,
				Message = entity.Message,
				Link = entity.LinkUrl,
				CreatedAt = DateTime.UtcNow,
				IsRead = entity.IsRead
			};

			await _realtime.PushNotificationsAsync(new[] { userId }, new[] { payload }, cancellationToken);
		}
	}
}
