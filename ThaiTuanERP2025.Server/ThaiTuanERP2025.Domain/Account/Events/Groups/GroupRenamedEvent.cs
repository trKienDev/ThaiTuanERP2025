using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.Groups
{

	public sealed class GroupRenamedEvent : IDomainEvent
	{
		public GroupRenamedEvent(Group group)
		{
			Group = group;
			OccurredOn = DateTime.UtcNow;
		}

		public Group Group { get; }
		public DateTime OccurredOn { get; }
	}
}
