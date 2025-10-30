using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.Users
{
	public sealed class UserSuperAdminChangedEvent : IDomainEvent
	{
		public UserSuperAdminChangedEvent(User user, bool isSuperAdmin)
		{
			User = user;
			IsSuperAdmin = isSuperAdmin;
			OccurredOn = DateTime.UtcNow;
		}

		public User User { get; }
		public bool IsSuperAdmin { get; }
		public DateTime OccurredOn { get; }
	}
}
