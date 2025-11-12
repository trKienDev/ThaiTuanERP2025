namespace ThaiTuanERP2025.Application.Core.Reminders
{
	public sealed record UserReminderDto {
		public Guid Id { get; init; }
		public string Subject { get; init; } = string.Empty;
		public string Message { get; init; } = string.Empty;
		public int SlaHours { get; init; }
		public DateTime DueAt { get; init; }
		public bool IsResolved { get; init; }
		public DateTime? ResolvedAt { get; init; }
		public string? LinkUrl { get; init; }
	};
}
