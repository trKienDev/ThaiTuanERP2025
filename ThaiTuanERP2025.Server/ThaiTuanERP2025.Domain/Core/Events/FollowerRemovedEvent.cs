using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Domain.Core.Events
{

	public sealed class FollowerRemovedEvent : IDomainEvent
	{
		public FollowerRemovedEvent(Follower follower)
		{
			Follower = follower;
			OccurredOn = DateTime.UtcNow;
		}

		public Follower Follower { get; }
		public DateTime OccurredOn { get; }
	}
}
