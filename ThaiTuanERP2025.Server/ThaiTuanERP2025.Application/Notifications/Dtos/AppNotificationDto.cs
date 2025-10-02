namespace ThaiTuanERP2025.Application.Notifications.Dtos
{
	public sealed record AppNotificationDto(
		Guid Id, 
		string Title,
		string Message,
		string Link,
		DateTime CreatedAt
	);
}
