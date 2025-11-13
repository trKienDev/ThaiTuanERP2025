using ThaiTuanERP2025.Domain.Core.Enums;

namespace ThaiTuanERP2025.Application.Core.Notifications
{
	public interface INotificationService
	{
		Task SendAsync(
			Guid senderId, Guid receiverId, string title, string message,
			NotificationLinkType linkType, Guid? targetId, NotificationType type = NotificationType.Info,
			CancellationToken cancellationToken = default
		);

		Task SendToManyAsync(
			Guid senderId,
			IEnumerable<Guid> userIds,
			string title,
			string message,
			NotificationLinkType linkType,
			Guid? targetId = null,
			NotificationType type = NotificationType.Info,
			CancellationToken cancellationToken = default
		);
	}
}
