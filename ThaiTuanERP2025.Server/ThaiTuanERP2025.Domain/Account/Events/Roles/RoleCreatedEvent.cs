using System;
using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Domain.Account.Events.Roles
{
	public sealed class RoleCreatedEvent : IDomainEvent
	{
		public Role Role { get; }
		public DateTime OccurredOn { get; } = DateTime.UtcNow;

		public RoleCreatedEvent(Role role)
		{
			Role = role;
		}
	}
}
