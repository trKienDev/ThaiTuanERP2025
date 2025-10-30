using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.Permissions
{
	public sealed class PermissionDescriptionUpdatedEvent : IDomainEvent
	{
		public PermissionDescriptionUpdatedEvent(Permission permission)
		{
			Permission = permission;
			OccurredOn = DateTime.UtcNow;
		}

		public Permission Permission { get; }
		public DateTime OccurredOn { get; }
	}
}
