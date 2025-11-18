using ThaiTuanERP2025.Domain.Core.Enums;

namespace ThaiTuanERP2025.Application.Core.Notifications.Contracts
{
	public sealed record NotificationCreatedPayload(
		Guid SenderId,
		Guid ReceiverId,
		Guid NotificationId,
		string Title,
		string Message,
		string LinkUrl,
		LinkType LinkType,
		Guid? TargetId,
		NotificationType Type,
		DateTime CreatedAt,
		bool IsRead
	);

	public sealed record NotificationReadPayload(
		Guid Id, 
		Guid ReceiverId,
		DateTime ReadAt
	);
}
