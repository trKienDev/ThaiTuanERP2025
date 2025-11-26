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

		#region Send to single user
		public async Task SendAsync(
			Guid senderId, Guid receiverId, string title, string message,
			LinkType linkType, Guid? targetId, NotificationType type = NotificationType.Info, 
			CancellationToken cancellationToken = default
		) {
			var entity = new UserNotification(senderId, receiverId, title, message, linkType, targetId, type);
			await _uow.UserNotifications.AddAsync(entity, cancellationToken);

			var senderDto = await _userRepo.GetBriefWithAvatarAsync(senderId, cancellationToken);
			if (senderDto is null)
				throw new DomainException($"User sender with id {senderId} không tồn tại.");
			
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
				var senderDto = await _userRepo.GetBriefWithAvatarAsync(entity.SenderId, cancellationToken)
					?? throw new NotFoundException("Không tìm thấy dữ liệu người gửi");

				var payload = new NotificationCreatedPayload(
					senderId,
					senderDto,
					entity.ReceiverId,
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
				await _uow.OutboxMessages.AddAsync(outbox, cancellationToken);
			}

			await _uow.SaveChangesWithoutDispatchAsync(cancellationToken);
		}
		#endregion

		#region MarkAsRead
		public async Task MarkAsReadAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken = default)
		{
			// Lấy notification
			var entity = await _uow.UserNotifications.SingleOrDefaultAsync(
				q => q.Where(x => x.Id == notificationId && x.ReceiverId == userId),
				asNoTracking: false,
				cancellationToken: cancellationToken
			);

			if (entity == null)
				throw new NotFoundException("Notification not found");

			// Domain behavior
			entity.MarkAsRead();

			// (Optionally) gửi Outbox event MarkRead để sync realtime
			var payload = new NotificationReadPayload(
				entity.Id,
				entity.ReceiverId,
				entity.ReadAt!.Value
			);

			var json = JsonSerializer.Serialize(payload);
			var outbox = new OutboxMessage("NotificationRead", json);

			await _uow.OutboxMessages.AddAsync(outbox, cancellationToken);

			await _uow.SaveChangesWithoutDispatchAsync(cancellationToken);
		}
		#endregion
	}
}
