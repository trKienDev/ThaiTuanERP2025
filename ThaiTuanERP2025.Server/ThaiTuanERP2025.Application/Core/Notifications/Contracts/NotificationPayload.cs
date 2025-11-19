using ThaiTuanERP2025.Application.Account.Users;
using ThaiTuanERP2025.Domain.Core.Enums;

namespace ThaiTuanERP2025.Application.Core.Notifications.Contracts
{
	public sealed record NotificationCreatedPayload(
		Guid SenderId,
		UserBriefAvatarDto Sender,
		Guid ReceiverId,
		Guid NotificationId,
		string Title,
		string Message,
		LinkType LinkType,
		NotificationType Type,
		DateTime CreatedAt,
		bool IsRead,
		Guid? TargetId,
		string? LinkUrl
	);

	public sealed record NotificationReadPayload(
		Guid Id, 
		Guid ReceiverId,
		DateTime ReadAt
	);
}
