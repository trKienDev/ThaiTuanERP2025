namespace ThaiTuanERP2025.Application.Core.Notifications
{
	public sealed record UserNotificationDto {
		public Guid Id { get; init; }
		public string Title { get; init; } = default!;
		public string Message { get; init; } = default!;
		public string? Link { get; init; }
		public DateTime CreatedAt { get; init; }
		public bool IsRead { get; init; }
	}
}
