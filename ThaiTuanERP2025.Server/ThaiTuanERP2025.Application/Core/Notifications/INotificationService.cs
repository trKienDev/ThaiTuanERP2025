using ThaiTuanERP2025.Domain.Core.Enums;

namespace ThaiTuanERP2025.Application.Core.Notifications
{
	public interface INotificationService
	{
		Task SendAsync(Guid userId, string title, string message, string? linkUrl = null, NotificationType type = NotificationType.Info, CancellationToken cancellationToken = default);
	}
}
