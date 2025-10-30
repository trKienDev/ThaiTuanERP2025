using MediatR;

namespace ThaiTuanERP2025.Domain.Common.Events
{
	/// <summary>
	/// Marker interface để xác định các Domain Event trong DDD.
	/// Không chứa logic, chỉ giúp các Event Dispatcher nhận diện event từ Domain.
	/// </summary>
	public interface IDomainEvent : INotification
	{
		DateTime OccurredOn { get; }
	}
}
