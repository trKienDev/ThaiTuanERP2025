using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Core.Enums;

namespace ThaiTuanERP2025.Domain.Core.Entities
{
	public class UserNotification : BaseEntity
	{
		#region EF Constructor
		private UserNotification() { }
		public UserNotification(Guid senderId, Guid receiverId, string title, string message, NotificationLinkType linkType, Guid? targetId = null, NotificationType type = NotificationType.Info)
		{
			Guard.AgainstDefault(receiverId, nameof(receiverId));
			Guard.AgainstDefault(senderId, nameof(senderId));
			Guard.AgainstNullOrWhiteSpace(title, nameof(title));

			Id = Guid.NewGuid();
			
			Title = title;
			SenderId = senderId;
			ReceiverId = receiverId;
			Message = message;
			LinkUrl = ResolveLink(linkType, targetId);
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
		public NotificationLinkType LinkType { get; private set; }

		/// <summary>ID của đối tượng gốc (BudgetPlanId, ExpenseId, RequestId...)</summary>
		public Guid? TargetId { get; private set; }

		public bool IsRead { get; private set; }
		public DateTime? ReadAt { get; private set; }
		public NotificationType Type { get; private set; } = NotificationType.Info;
		public DateTime CreatedAt { get; private set; }

		#endregion

		#region Domain Behaviors
		public void MarkAsRead()
		{
			if (!IsRead)
			{
				IsRead = true;
				ReadAt = DateTime.UtcNow;
			}
		}
		#endregion

		private string? ResolveLink(NotificationLinkType type, Guid? id)
		{
			return type switch
			{
				NotificationLinkType.BudgetPlanReview when id.HasValue 
					=> NotificationLinks.BudgetPlanReview(id.Value),

				NotificationLinkType.BudgetPlanDetail when id.HasValue
					=> NotificationLinks.BudgetPlanDetail(id.Value),

				NotificationLinkType.ExpensePaymentDetail when id.HasValue
					=> NotificationLinks.ExpensePaymentDetail(id.Value),

				NotificationLinkType.RequestDetail when id.HasValue
					=> NotificationLinks.RequestDetail(id.Value),

				NotificationLinkType.Dashboard
					=> NotificationLinks.Dashboard(),

				_ => null
			};
		}
	}
}
