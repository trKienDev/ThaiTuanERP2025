using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.Roles
{
	public sealed class RoleUpdatedEvent : IDomainEvent
	{
		public Role Role { get; }
		public DateTime OccurredOn { get; } = DateTime.UtcNow;

		public RoleUpdatedEvent(Role role)
		{
			Role = role;
		}
	}
}
