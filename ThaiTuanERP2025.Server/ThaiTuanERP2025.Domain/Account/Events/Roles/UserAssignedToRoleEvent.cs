using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.Roles
{
	public sealed class UserAssignedToRoleEvent : IDomainEvent
	{
		public Role Role { get; }
		public Guid UserId { get; }
		public DateTime OccurredOn { get; } = DateTime.UtcNow;

		public UserAssignedToRoleEvent(Role role, Guid userId)
		{
			Role = role;
			UserId = userId;
		}
	}
}
