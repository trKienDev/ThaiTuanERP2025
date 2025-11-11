using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Core.Enums;

namespace ThaiTuanERP2025.Application.Core.Services
{
	public interface INotificationService
	{
		Task SendAsync(Guid userId, string title, string message, string? linkUrl = null, NotificationType type = NotificationType.Info, CancellationToken cancellationToken = default);
	}

	public class NotificationService : INotificationService
	{
		private readonly IUnitOfWork _uow;
		public NotificationService(IUnitOfWork uow) {
			_uow = uow;	
		}

		public async Task SendAsync(Guid userId, string title, string message, string? linkUrl = null, NotificationType type = NotificationType.Info, CancellationToken cancellationToken = default)
		{
			var entity = new UserNotification(userId, title, message, linkUrl, type);
			await _uow.UserNotifications.AddAsync(entity, cancellationToken);
			await _uow.SaveChangesAsync(cancellationToken);
		}
	}
}
