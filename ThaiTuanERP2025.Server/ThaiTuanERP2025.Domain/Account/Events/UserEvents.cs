using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events
{
	public abstract class UserEventBase : IDomainEvent
	{
		public Guid UserId { get; }
		public DateTime OccurredOn { get; }
		protected UserEventBase(Guid userId) {
			UserId = userId;
			OccurredOn = DateTime.UtcNow;
		}
	}

	public sealed class UserActivatedEvent : UserEventBase
	{
		public UserActivatedEvent(User user) : base(user.Id)
		{
			User = user;
		}
		public User User { get; }
	}

	public sealed class UserAssignedRoleEvent : UserEventBase
	{
		public UserAssignedRoleEvent(User user, Guid roleId) : base(user.Id) 
		{
			User = user;
			RoleId = roleId;
		}

		public User User { get; }
		public Guid RoleId { get; }
	}

	public sealed class UserAvatarUpdatedEvent : UserEventBase
	{
		public UserAvatarUpdatedEvent(User user) : base(user.Id)
		{
			User = user;
		}
		public User User { get; }
	}

	public sealed class UserCreatedEvent : UserEventBase
	{
		public UserCreatedEvent(User user) : base(user.Id)
		{
			User = user;
		}
		public User User { get; }
	}

	public sealed class UserDeactivatedEvent : UserEventBase
	{
		public UserDeactivatedEvent(User user) : base(user.Id)
		{
			User = user;
		}
		public User User { get; }
	}

	public sealed class UserDepartmentChangedEvent : UserEventBase
	{
		public UserDepartmentChangedEvent(User user, Guid departmentId) : base(user.Id)
		{
			User = user;
			DepartmentId = departmentId;
		}

		public User User { get; }
		public Guid DepartmentId { get; }
	}

	public sealed class UserManagerAssignedEvent : UserEventBase
	{
		public UserManagerAssignedEvent(User user, Guid managerId) : base(user.Id)
		{
			User = user;
			ManagerId = managerId;
		}
		public User User { get; }
		public Guid ManagerId { get; }
	}

	public sealed class UserPasswordChangedEvent : UserEventBase
	{
		public UserPasswordChangedEvent(User user) : base(user.Id)
		{
			User = user;
		}
		public User User { get; }
	}

	public sealed class UserProfileUpdatedEvent : UserEventBase
	{
		public UserProfileUpdatedEvent(User user) : base(user.Id)
		{
			User = user;
		}

		public User User { get; }
	}

	public sealed class UserRemovedRoleEvent : UserEventBase
	{
		public UserRemovedRoleEvent(User user, Guid roleId) : base(user.Id)
		{
			User = user;
			RoleId = roleId;
		}

		public User User { get; }
		public Guid RoleId { get; }
	}

	public sealed class UserSuperAdminChangedEvent : UserEventBase
	{
		public UserSuperAdminChangedEvent(User user, bool isSuperAdmin) : base(user.Id)
		{
			User = user;
			IsSuperAdmin = isSuperAdmin;
		}
		public User User { get; }
		public bool IsSuperAdmin { get; }
	}

	public sealed class UserPermanentDeleteRequestedEvent : UserEventBase
	{
		public UserPermanentDeleteRequestedEvent(User user) : base(user.Id)
		{
			User = user;
		}
		public User User { get; }
		public bool IsSuperAdmin { get; }
	}
}
