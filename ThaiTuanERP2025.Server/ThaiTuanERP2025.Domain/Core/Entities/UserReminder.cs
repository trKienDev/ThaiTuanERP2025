using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;

namespace ThaiTuanERP2025.Domain.Core.Entities
{
	public class UserReminder : BaseEntity
	{
		#region EF Constructor
		private UserReminder() { }
		public UserReminder(Guid userId, string subject, string message, int slaHours, DateTime dueAt, string? linkUrl = null)
		{
			Guard.AgainstDefault(userId, nameof(userId));
			Guard.AgainstNullOrWhiteSpace(subject, nameof(subject));

			Id = Guid.NewGuid();
			UserId = userId;
			Subject = subject;
			Message = message;
			LinkUrl = linkUrl;
			SlaHours = slaHours;
			DueAt = dueAt;
			IsResolved = false;
			CreatedAt = DateTime.UtcNow;
		}
		#endregion


		public Guid UserId { get; private set; }
		public User User { get; private set; } = null!;

		/// <summary>Nội dung nhắc việc</summary>
		public string Subject { get; private set; } = string.Empty;
		public string Message { get; private set; } = string.Empty;

		public string? LinkUrl { get; private set; }

		public int SlaHours { get; private set; }
		public DateTime DueAt { get; private set; }


		public bool IsResolved { get; private set; }
		public DateTime? ResolvedAt { get; private set; }

		public DateTime CreatedAt { get; private set; }

		public void MarkResolved()
		{
			IsResolved = true;
			ResolvedAt = DateTime.UtcNow;
		}
	}
}
