using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Domain.Core.Events
{
	public sealed class FollowerReactivatedEvent : IDomainEvent
	{
		public FollowerReactivatedEvent(Follower follower)
		{
			Follower = follower;
			OccurredOn = DateTime.UtcNow;
		}

		public Follower Follower { get; }
		public DateTime OccurredOn { get; }
	}
}
