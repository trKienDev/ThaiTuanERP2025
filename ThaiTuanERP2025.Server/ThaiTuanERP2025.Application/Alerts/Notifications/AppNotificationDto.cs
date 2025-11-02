namespace ThaiTuanERP2025.Application.Alerts.Notifications
{
	public sealed record AppNotificationDto(
		Guid Id, 
		string Title,
		string Message,
		string? Link,
		DateTime CreatedAt,
		bool IsRead
	);
}
