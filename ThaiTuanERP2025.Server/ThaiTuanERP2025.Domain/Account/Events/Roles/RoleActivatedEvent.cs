using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.Roles
{
	public sealed class RoleActivatedEvent : IDomainEvent
	{
		public Role Role { get; }
		public DateTime OccurredOn { get; } = DateTime.UtcNow;

		public RoleActivatedEvent(Role role)
		{
			Role = role;
		}
	}
}
