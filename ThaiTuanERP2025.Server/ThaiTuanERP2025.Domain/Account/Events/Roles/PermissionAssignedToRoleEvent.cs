using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.Roles
{
	public sealed class PermissionAssignedToRoleEvent : IDomainEvent
	{
		public Role Role { get; }
		public Guid PermissionId { get; }
		public DateTime OccurredOn { get; } = DateTime.UtcNow;

		public PermissionAssignedToRoleEvent(Role role, Guid permissionId)
		{
			Role = role;
			PermissionId = permissionId;
		}
	}
}
