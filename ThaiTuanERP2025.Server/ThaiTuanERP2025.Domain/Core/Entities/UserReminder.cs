using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Common.Entities;

namespace ThaiTuanERP2025.Domain.Core.Entities
{
	public class UserReminder : BaseEntity
	{
		#region EF Constructor
		private UserReminder() { }
		public UserReminder(Guid userId, string subject, string message, DateTime triggerAt, string? linkUrl = null)
		{
			Guard.AgainstDefault(userId, nameof(userId));
			Guard.AgainstNullOrWhiteSpace(subject, nameof(subject));

			Id = Guid.NewGuid();
			UserId = userId;
			Subject = subject;
			Message = message;
			TriggerAt = triggerAt;
			LinkUrl = linkUrl;
			IsTriggered = false;
		}
		#endregion


		public Guid UserId { get; private set; }
		public User User { get; private set; } = null!;

		/// <summary>Nội dung nhắc việc</summary>
		public string Subject { get; private set; } = string.Empty;

		public string Message { get; private set; } = string.Empty;

		/// <summary>Thời điểm sẽ kích hoạt nhắc việc</summary>
		public DateTime TriggerAt { get; private set; }

		/// <summary>Trạng thái đã kích hoạt chưa</summary>
		public bool IsTriggered { get; private set; }

		/// <summary>Nếu đã kích hoạt, thời điểm kích hoạt thực tế</summary>
		public DateTime? TriggeredAt { get; private set; }

		/// <summary>Nếu cần link chi tiết (mở task cụ thể)</summary>
		public string? LinkUrl { get; private set; }

		public void MarkTriggered()
		{
			IsTriggered = true;
			TriggeredAt = DateTime.UtcNow;
		}
	}
}
