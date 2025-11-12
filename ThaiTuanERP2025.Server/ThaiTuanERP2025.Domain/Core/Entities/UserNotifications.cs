using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Common.Entities;
using ThaiTuanERP2025.Domain.Core.Enums;

namespace ThaiTuanERP2025.Domain.Core.Entities
{
	public class UserNotification : BaseEntity
	{
		#region EF Constructor
		private UserNotification() { }
		public UserNotification(Guid userId, string title, string message, string? linkUrl = null, NotificationType type = NotificationType.Info)
		{
			Guard.AgainstDefault(userId, nameof(userId));
			Guard.AgainstNullOrWhiteSpace(title, nameof(title));

			Id = Guid.NewGuid();
			UserId = userId;
			Title = title;
			Message = message;
			LinkUrl = linkUrl;
			Type = type;
			IsRead = false;
			CreatedAt = DateTime.UtcNow;
		}
		#endregion

		#region Properties
		public Guid UserId { get; private set; }
		public User User { get; private set; } = null!;

		/// <summary>Tiêu đề ngắn (ví dụ: "Kế hoạch ngân sách mới")</summary>
		public string Title { get; private set; } = string.Empty;

		/// <summary>Nội dung chi tiết (có thể chứa link hoặc mã kế hoạch)</summary>
		public string Message { get; private set; } = string.Empty;

		/// <summary>Đường dẫn trong app để mở chi tiết (optional)</summary>
		public string? LinkUrl { get; private set; }

		/// <summary>Trạng thái đã đọc</summary>
		public bool IsRead { get; private set; }

		/// <summary>Thời điểm đọc</summary>
		public DateTime? ReadAt { get; private set; }

		/// <summary>Loại thông báo: Info / Warning / Task / Approval / System...</summary>
		public NotificationType Type { get; private set; } = NotificationType.Info;

		/// <summary>Ngày tạo</summary>
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
	}
}
