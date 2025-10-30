using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.Users
{
	public sealed class UserManagerAssignedEvent : IDomainEvent
	{
		public UserManagerAssignedEvent(User user, Guid managerId)
		{
			User = user;
			ManagerId = managerId;
			OccurredOn = DateTime.UtcNow;
		}

		public User User { get; }
		public Guid ManagerId { get; }
		public DateTime OccurredOn { get; }
	}
}
