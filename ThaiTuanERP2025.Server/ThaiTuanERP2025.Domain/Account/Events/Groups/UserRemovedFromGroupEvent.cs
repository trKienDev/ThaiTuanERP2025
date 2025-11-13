using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.Groups
{
	public sealed class UserRemovedFromGroupEvent : IDomainEvent
	{
		public UserRemovedFromGroupEvent(Group group, Guid userId)
		{
			Group = group;
			UserId = userId;
			OccurredOn = DateTime.UtcNow;
		}

		public Group Group { get; }
		public Guid UserId { get; }
		public DateTime OccurredOn { get; }
	}
}
