using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.Groups
{
	public sealed class UserAddedToGroupEvent : IDomainEvent
	{
		public UserAddedToGroupEvent(Group group, Guid userId)
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
