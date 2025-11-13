using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.Groups
{
	public sealed class GroupCreatedEvent : IDomainEvent
	{
		public GroupCreatedEvent(Group group)
		{
			Group = group;
			OccurredOn = DateTime.UtcNow;
		}

		public Group Group { get; }
		public DateTime OccurredOn { get; }
	}
}
