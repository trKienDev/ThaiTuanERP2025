using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.Groups
{
	public sealed class GroupDescriptionUpdatedEvent : IDomainEvent
	{
		public GroupDescriptionUpdatedEvent(Group group)
		{
			Group = group;
			OccurredOn = DateTime.UtcNow;
		}

		public Group Group { get; }
		public DateTime OccurredOn { get; }
	}
}
