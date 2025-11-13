using MediatR;

namespace ThaiTuanERP2025.Application.Shared.Events
{
	/// <summary>
	/// Đánh dấu 1 Application-level event (Command Event) xảy ra sau khi một Use Case hoặc Command được xử lý thành công.
	/// </summary>
	public interface IApplicationEvent : INotification
	{
		/// <summary>
		/// Thời điểm sự kiện được sinh ra (UTC).
		/// </summary>
		DateTime OccurredOn { get; }
	}
}
