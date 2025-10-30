using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.Users
{
	public sealed class UserCreatedEvent : IDomainEvent
	{
		public UserCreatedEvent(User user)
		{
			User = user;
			OccurredOn = DateTime.UtcNow;
		}

		public User User { get; }
		public DateTime OccurredOn { get; }
	}
}
