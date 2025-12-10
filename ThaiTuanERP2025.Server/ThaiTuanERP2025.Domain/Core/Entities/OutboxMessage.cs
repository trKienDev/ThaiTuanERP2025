using ThaiTuanERP2025.Domain.Shared.Entities;

namespace ThaiTuanERP2025.Domain.Core.Entities
{
	public class OutboxMessage : BaseEntity
	{
		#region EF Constructor
		private OutboxMessage() { }
		public OutboxMessage(string type, string payload)
		{
			Type = type ?? throw new ArgumentNullException(nameof(type));
			Payload = payload ?? throw new ArgumentNullException(nameof(payload));
			OccurredOnUtc = DateTime.UtcNow;
		}
		#endregion

		#region Properties
		public string Type { get; private set; } = default!;  // Ví dụ: "ReminderCreated"
		public string Payload { get; private set; } = default!; // JSON
		public DateTime OccurredOnUtc { get; private set; }

		public DateTime? ProcessedOnUtc { get; private set; }
		public string? Error { get; private set; }
		public int RetryCount { get; private set; }
		#endregion

		#region Domain Behaviors
		public void MarkProcessed()
		{
			ProcessedOnUtc = DateTime.UtcNow;
			Error = null;
		}

		public void MarkFailed(string error)
		{
			RetryCount++;
			Error = error;
		}
		#endregion
	}
}
