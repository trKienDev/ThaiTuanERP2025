using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.Users
{
	public sealed class UserDeactivatedEvent : IDomainEvent
	{
		public UserDeactivatedEvent(User user)
		{
			User = user;
			OccurredOn = DateTime.UtcNow;
		}

		public User User { get; }
		public DateTime OccurredOn { get; }
	}
}
