using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Core.Enums;
using ThaiTuanERP2025.Domain.Core.Utilities;

namespace ThaiTuanERP2025.Domain.Core.Entities
{
	public class UserNotification : BaseEntity
	{
		#region EF Constructor
		private UserNotification() { }
		public UserNotification(Guid senderId, Guid receiverId, string title, string message, LinkType linkType, Guid? targetId = null, NotificationType type = NotificationType.Info)
		{
			Guard.AgainstDefault(receiverId, nameof(receiverId));
			Guard.AgainstDefault(senderId, nameof(senderId));
			Guard.AgainstNullOrWhiteSpace(title, nameof(title));

			Id = Guid.NewGuid();
			
			Title = title;
			SenderId = senderId;
			ReceiverId = receiverId;
			Message = message;
			LinkUrl = AppLinkResolver.Resolve(linkType, targetId);
			TargetId = targetId;
			LinkType = linkType;
			Type = type;
			IsRead = false;
			CreatedAt = DateTime.UtcNow;
		}
		#endregion

		#region Properties
		public Guid ReceiverId { get; private set; }
		public User Receiver { get; } = default!;

		public Guid SenderId { get; private set; }
		public User Sender { get; } = default!;

		public string Title { get; private set; } = string.Empty;
		public string Message { get; private set; } = string.Empty;

		/// <summary>Đường dẫn nội bộ Angular</summary>
		public string? LinkUrl { get; private set; }

		/// <summary>Loại link để UI có thể render icon hoặc điều hướng đặc thù</summary>
		public LinkType LinkType { get; private set; }

		/// <summary>ID của đối tượng gốc (BudgetPlanId, ExpenseId, RequestId...)</summary>
		public Guid? TargetId { get; private set; }

		public bool IsRead { get; private set; }
		public DateTime? ReadAt { get; private set; }
		public NotificationType Type { get; private set; } = NotificationType.Info;
		public DateTime CreatedAt { get; private set; }

		#endregion

		#region Domain Behaviors
		internal void MarkAsRead()
		{
			if (!IsRead)
			{
				IsRead = true;
				ReadAt = DateTime.UtcNow;
			}
		}
		#endregion
	}
}
