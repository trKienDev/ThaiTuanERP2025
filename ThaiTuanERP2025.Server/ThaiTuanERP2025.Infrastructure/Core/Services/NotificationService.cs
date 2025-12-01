using System.Text.Json;
using ThaiTuanERP2025.Application.Account.Users.Repositories;
using ThaiTuanERP2025.Application.Core.Notifications;
using ThaiTuanERP2025.Application.Core.Notifications.Contracts;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Core.Enums;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Core.Services
{
	public class NotificationService : INotificationService
	{
		private readonly IUnitOfWork _uow;
		private readonly IUserReadRepostiory _userRepo;
		public NotificationService(IUnitOfWork uow, IUserReadRepostiory userRepo) {
			_uow = uow;	
			_userRepo = userRepo;	
		}

		#region Send
		public async Task SendAsync(
			Guid senderId, Guid receiverId, string title, string message,
			LinkType linkType, Guid? targetId, NotificationType type = NotificationType.Info, 
			CancellationToken cancellationToken = default
		) {
			var (entity, outbox) = await CreateNotificationInternalAsync(
			senderId, receiverId, title, message, linkType, targetId, type, cancellationToken);

			await _uow.UserNotifications.AddAsync(entity, cancellationToken);
			await _uow.OutboxMessages.AddAsync(outbox, cancellationToken);

			await _uow.SaveChangesWithoutDispatchAsync(cancellationToken);
		}

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
			if (!receivers.Any())
				return;

			var notifications = new List<UserNotification>();
			var outboxes = new List<OutboxMessage>();

			foreach (var receiverId in receivers)
			{
				var (entity, outbox) = await CreateNotificationInternalAsync(
				    senderId, receiverId, title, message, linkType, targetId, type, cancellationToken);

				notifications.Add(entity);
				outboxes.Add(outbox);
			}

			await _uow.UserNotifications.AddRangeAsync(notifications, cancellationToken);
			await _uow.OutboxMessages.AddRangeAsync(outboxes, cancellationToken);

			await _uow.SaveChangesWithoutDispatchAsync(cancellationToken);
		}

		// === PRIVATE ===
		private async Task<(UserNotification entity, OutboxMessage outbox)> CreateNotificationInternalAsync( Guid senderId, Guid receiverId, string title, string message, LinkType linkType, Guid? targetId, NotificationType type, CancellationToken cancellationToken)
		{
			// === Entity ===
			var entity = new UserNotification(senderId, receiverId, title, message, linkType, targetId, type);

			// === Sender DTO ===
			var senderDto = await _userRepo.GetBriefWithAvatarAsync(senderId, cancellationToken)
			    ?? throw new DomainException($"User sender with id {senderId} không tồn tại.");

			// === Payload ===
			var payload = new NotificationCreatedPayload(
				senderId,
				senderDto,
				receiverId,
				entity.Id,
				entity.Title,
				entity.Message,
				entity.LinkType,
				entity.Type,
				entity.CreatedAt,
				entity.IsRead,
				entity.TargetId,
				entity.LinkUrl
			);

			var json = JsonSerializer.Serialize(payload);

			var outbox = new OutboxMessage("NotificationCreated", json);

			return (entity, outbox);
		}
		#endregion

		#region Mark
		// === PUBLIC ===
		public async Task MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken = default)
		{
			var (_, outbox) = await MarkAsReadInternalAsync(notificationId, cancellationToken);

			await _uow.OutboxMessages.AddAsync(outbox, cancellationToken);
			await _uow.SaveChangesWithoutDispatchAsync(cancellationToken);
		}

		public async Task MarkAsReadManyAsync(IEnumerable<Guid> notificationIds, CancellationToken cancellationToken = default)
		{
			if (notificationIds == null || !notificationIds.Any())
				return;

			var outboxes = new List<OutboxMessage>();

			foreach (var id in notificationIds.Distinct())
			{
				var (_, outbox) = await MarkAsReadInternalAsync(id, cancellationToken);
				outboxes.Add(outbox);
			}

			await _uow.OutboxMessages.AddRangeAsync(outboxes, cancellationToken);
			await _uow.SaveChangesWithoutDispatchAsync(cancellationToken);
		}

		// ==== PRIVATE ====
		private async Task<(UserNotification entity, OutboxMessage outbox)> MarkAsReadInternalAsync(Guid notificationId, CancellationToken cancellationToken)
		{
			// 1. Lấy notification
			var entity = await _uow.UserNotifications.GetByIdAsync(notificationId, cancellationToken);

			if (entity == null)
				throw new NotFoundException("Notification not found");

			// 2. Domain behavior
			entity.MarkAsRead();

			// 3. Outbox
			var payload = new NotificationReadPayload(
			    entity.Id,
			    entity.ReceiverId,
			    entity.ReadAt!.Value
			);

			var json = JsonSerializer.Serialize(payload);
			var outbox = new OutboxMessage("NotificationRead", json);

			return (entity, outbox);
		}

		#endregion
	}
}
