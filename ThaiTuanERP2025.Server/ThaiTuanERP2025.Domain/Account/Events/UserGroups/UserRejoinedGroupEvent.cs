using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.UserGroups
{
	public sealed class UserRejoinedGroupEvent : IDomainEvent
	{
		public UserRejoinedGroupEvent(Guid userId, Guid groupId)
		{
			UserId = userId;
			GroupId = groupId;
			OccurredOn = DateTime.UtcNow;
		}

		public Guid UserId { get; }
		public Guid GroupId { get; }
		public DateTime OccurredOn { get; }
	}
}
