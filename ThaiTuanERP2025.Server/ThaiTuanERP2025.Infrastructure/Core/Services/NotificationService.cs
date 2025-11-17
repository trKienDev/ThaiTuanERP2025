using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Application.Core.Notifications;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Core.Enums;
using ThaiTuanERP2025.Domain.Shared.Repositories;

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

		#region Send to single user
		public async Task SendAsync(
			Guid senderId, Guid receiverId, string title, string message,
			LinkType linkType, Guid? targetId, NotificationType type = NotificationType.Info, 
			CancellationToken cancellationToken = default
		) {
			var entity = new UserNotification(senderId, receiverId, title, message, linkType, targetId, type);

			await _uow.UserNotifications.AddAsync(entity, cancellationToken);
			await _uow.SaveChangesWithoutDispatchAsync(cancellationToken);

			// push realtime
			var payload = new UserNotificationDto {
				Id = entity.Id,
				SenderId = entity.SenderId,
				Title = entity.Title,
				Message = entity.Message,
				Link = entity.LinkUrl,
				LinkType = entity.LinkType,
				TargetId = entity.TargetId,
				Type = entity.Type,
				CreatedAt = DateTime.UtcNow,
				IsRead = entity.IsRead
			};

			await _realtime.PushNotificationsAsync(
				new[] { receiverId }, new[] { payload }, cancellationToken
			);
		}
		#endregion

		#region Send to many users
		public async Task SendToManyAsync(
			Guid senderId,
			IEnumerable<Guid> userIds,
			string title,
			string message,
			LinkType linkType,
			Guid? targetId = null,
			NotificationType type = NotificationType.Info,
			CancellationToken cancellationToken = default
		) {
			var userList = userIds.Distinct().ToList();
			if (!userList.Any()) return;

			// Fan-out entity
			var notifications = userList.Select(
				uid => new UserNotification (
					senderId, uid,
					title, message,
					linkType,
					targetId,
					type
				)
			).ToList();

			await _uow.UserNotifications.AddRangeAsync(notifications, cancellationToken);
			await _uow.SaveChangesWithoutDispatchAsync(cancellationToken);

			// DTO payloads
			var payloads = notifications.Select(
				entity => new UserNotificationDto {
					Id = entity.Id,
					Title = entity.Title,
					Message = entity.Message,

					Link = entity.LinkUrl,
					LinkType = entity.LinkType,
					TargetId = entity.TargetId,

					Type = entity.Type,
					CreatedAt = entity.CreatedAt,
					IsRead = entity.IsRead
				}
			).ToList();

			// Push realtime to all users
			await _realtime.PushNotificationsAsync(
				userList,
				payloads,
				cancellationToken
			);
		}
		#endregion
	}
}
