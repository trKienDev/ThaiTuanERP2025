using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.Users
{
	public sealed class UserAssignedRoleEvent : IDomainEvent
	{
		public UserAssignedRoleEvent(User user, Guid roleId)
		{
			User = user;
			RoleId = roleId;
			OccurredOn = DateTime.UtcNow;
		}

		public User User { get; }
		public Guid RoleId { get; }
		public DateTime OccurredOn { get; }
	}
}
