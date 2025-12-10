using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared.Events;

namespace ThaiTuanERP2025.Domain.Account.Events
{
	public abstract class PermissionEvents : IDomainEvent
	{
		public Guid PermissionId { get; }
		public DateTime OccurredOn { get; }
		protected PermissionEvents(Guid permissionId)
		{
			PermissionId = permissionId;
			OccurredOn = DateTime.UtcNow;
		}
	}

	public sealed class PermissionActivatedEvent : PermissionEvents
	{
		public PermissionActivatedEvent(Permission permission) : base(permission.Id)
		{
			Permission = permission;
		}	
		public Permission Permission { get; }
	}

	public sealed class PermissionCreatedEvent : PermissionEvents
	{
		public PermissionCreatedEvent(Permission permission) : base(permission.Id)
		{
			Permission = permission;
		}
		public Permission Permission { get; }
	}

	public sealed class PermissionDeactivatedEvent : PermissionEvents
	{
		public PermissionDeactivatedEvent(Permission permission) : base(permission.Id)
		{
			Permission = permission;
		}
		public Permission Permission { get; }
	}

	public sealed class PermissionDescriptionUpdatedEvent : PermissionEvents
	{
		public PermissionDescriptionUpdatedEvent(Permission permission) : base(permission.Id)
		{
			Permission = permission;
		}
		public Permission Permission { get; }
	}

	public sealed class PermissionRenamedEvent : PermissionEvents
	{
		public PermissionRenamedEvent(Permission permission) : base(permission.Id)
		{
			Permission = permission;
		}
		public Permission Permission { get; }
	}
}
