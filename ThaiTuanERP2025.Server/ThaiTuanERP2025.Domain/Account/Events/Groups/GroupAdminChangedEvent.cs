using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.Groups
{
	public sealed class GroupAdminChangedEvent : IDomainEvent
	{
		public GroupAdminChangedEvent(Group group, Guid newAdminId)
		{
			Group = group;
			NewAdminId = newAdminId;
			OccurredOn = DateTime.UtcNow;
		}

		public Group Group { get; }
		public Guid NewAdminId { get; }
		public DateTime OccurredOn { get; }
	}
}
