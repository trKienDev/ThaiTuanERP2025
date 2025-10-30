using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.Roles
{
	public sealed class RoleDeactivatedEvent : IDomainEvent
	{
		public Role Role { get; }
		public DateTime OccurredOn { get; } = DateTime.UtcNow;

		public RoleDeactivatedEvent(Role role)
		{
			Role = role;
		}
	}
}
