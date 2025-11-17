using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Application.Core.Notifications;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Core.Enums;
using ThaiTuanERP2025.Domain.Shared.Repositories;
using ThaiTuanERP2025.Application.Core.Notifications.Contracts;
using System.Text.Json;

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

			var payload = new NotificationCreatedPayload(
				senderId,
				receiverId,
				entity.Id,
				entity.Title,
				entity.Message,
				entity.LinkUrl,
				entity.LinkType,
				entity.TargetId,
				entity.Type,
				entity.CreatedAt,
				entity.IsRead
			);

			var json = JsonSerializer.Serialize(payload);

			var outbox = new OutboxMessage("NotificationCreated", json);
			await _uow.OutboxMessages.AddAsync(outbox, cancellationToken);

			await _uow.SaveChangesWithoutDispatchAsync(cancellationToken);
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
			var receivers = userIds.Distinct().ToList();
			if (!receivers.Any()) return;

			var notifications = receivers.Select(
				uid => new UserNotification(senderId, uid, title, message, linkType, targetId, type)
			).ToList();

			await _uow.UserNotifications.AddRangeAsync(notifications, cancellationToken);

			foreach (var entity in notifications)
			{
				var payload = new NotificationCreatedPayload(
					senderId,
					entity.ReceiverId,
					entity.Id,
					entity.Title,
					entity.Message,
					entity.LinkUrl,
					entity.LinkType,
					entity.TargetId,
					entity.Type,
					entity.CreatedAt,
					entity.IsRead
				);

				var json = JsonSerializer.Serialize(payload);

				var outbox = new OutboxMessage("NotificationCreated", json);
				await _uow.OutboxMessages.AddAsync(outbox, cancellationToken);
			}

			await _uow.SaveChangesWithoutDispatchAsync(cancellationToken);
		}
		#endregion
	}
}
