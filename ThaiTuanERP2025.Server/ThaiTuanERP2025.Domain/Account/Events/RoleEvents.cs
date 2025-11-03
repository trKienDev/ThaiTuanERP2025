using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events
{
	public abstract class RoleEvents : IDomainEvent
	{
		public Guid RoleId { get; }
		public DateTime OccurredOn { get; }
		protected RoleEvents(Guid roleId)
		{
			RoleId = roleId;
			OccurredOn = DateTime.UtcNow;
		}
	}

	public sealed class RoleCreatedEvent : RoleEvents
	{
		public Role Role { get; }
		public RoleCreatedEvent(Role role) : base(role.Id)
		{
			Role = role;
		}
	}

	public sealed class PermissionAssignedToRoleEvent : RoleEvents
	{
		public Role Role { get; }
		public Guid PermissionId { get; }
		public PermissionAssignedToRoleEvent(Role role, Guid permissionId) : base(role.Id)
		{
			Role = role;
			PermissionId = permissionId;
		}
	}

	public sealed class PermissionRemovedFromRoleEvent : RoleEvents
	{
		public Role Role { get; }
		public Guid PermissionId { get; }
		public PermissionRemovedFromRoleEvent(Role role, Guid permissionId) : base(role.Id)
		{
			Role = role;
			PermissionId = permissionId;
		}
	}

	public sealed class RoleActivatedEvent : RoleEvents
	{
		public Role Role { get; }
		public RoleActivatedEvent(Role role) : base(role.Id)
		{
			Role = role;
		}
	}

	public sealed class RoleDeactivatedEvent : RoleEvents
	{
		public Role Role { get; }
		public RoleDeactivatedEvent(Role role) : base(role.Id)
		{
			Role = role;
		}
	}

	public sealed class RoleUpdatedEvent : RoleEvents
	{
		public Role Role { get; }
		public RoleUpdatedEvent(Role role) : base(role.Id)
		{
			Role = role;
		}
	}

	public sealed class UserAssignedToRoleEvent : RoleEvents
	{
		public Role Role { get; }
		public Guid UserId { get; }
		public UserAssignedToRoleEvent(Role role, Guid userId) : base(role.Id)
		{
			Role = role;
			UserId = userId;
		}
	}

	public sealed class UserRemovedFromRoleEvent : RoleEvents
	{
		public Role Role { get; }
		public Guid UserId { get; }
		public UserRemovedFromRoleEvent(Role role, Guid userId) : base(role.Id)
		{
			Role = role;
			UserId = userId;
		}
	}
}
