using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared.Events;

namespace ThaiTuanERP2025.Domain.Account.Events
{
	public abstract class DepartmentEvents : IDomainEvent
	{
		public Guid DepartmentId { get; }
		public DateTime OccurredOn { get; }
		protected DepartmentEvents(Guid departmentId)
		{
			DepartmentId = departmentId;
			OccurredOn = DateTime.UtcNow;
		}
	}

	public sealed class DepartmentActivatedEvent : DepartmentEvents
	{
		public DepartmentActivatedEvent(Department department) : base(department.Id)
		{
			Department = department;
		}
		public Department Department { get; }
	}

	public sealed class DepartmentChildAddedEvent : DepartmentEvents
	{
		public DepartmentChildAddedEvent(Department parent, Department child) : base(parent.Id)
		{
			Parent = parent;
			Child = child;
		}

		public Department Parent { get; }
		public Department Child { get; }
	}

	public sealed class DepartmentChildRemovedEvent : DepartmentEvents
	{
		public DepartmentChildRemovedEvent(Department parent, Department child) : base(parent.Id)
		{
			Parent = parent;
			Child = child;
		}

		public Department Parent { get; }
		public Department Child { get; }
	}

	public sealed class DepartmentCreatedEvent : DepartmentEvents
	{
		public DepartmentCreatedEvent(Department department) : base(department.Id)
		{
			Department = department;
		}

		public Department Department { get; }
	}

	public sealed class DepartmentDeactivatedEvent : DepartmentEvents
	{
		public DepartmentDeactivatedEvent(Department department) : base(department.Id)
		{
			Department = department;
		}
		public Department Department { get; }
	}


	public sealed class DepartmentRenamedEvent : DepartmentEvents
	{
		public DepartmentRenamedEvent(Department department) : base(department.Id)
		{
			Department = department;
		}
		public Department Department { get; }
	}

	public sealed class DepartmentParentChangedEvent : DepartmentEvents
	{
		public Department Department { get; }
		public Guid? OldParentId { get; }
		public Guid? NewParentId { get; }
		public DepartmentParentChangedEvent(Department department, Guid? oldParentId, Guid? newParentId) : base(department.Id)
		{
			Department = department;
			OldParentId = oldParentId;
			NewParentId = newParentId;
		}
	}

	public sealed class DepartmentManagerAddedEvent : DepartmentEvents
	{
		public DepartmentManagerAddedEvent(Department department, Guid userId, bool isPrimary = false)
			: base(department.Id)
		{
			Department = department;
			UserId = userId;
			IsPrimary = isPrimary;
		}

		public Department Department { get; }
		public Guid UserId { get; }
		// Trạng thái tại thời điểm thêm (thường là false, trừ khi bạn cho phép thêm với vai trò primary ngay lập tức).
		public bool IsPrimary { get; }
	}

	public sealed class DepartmentManagerRemovedEvent : DepartmentEvents
	{
		public DepartmentManagerRemovedEvent(Department department, Guid userId)
			: base(department.Id)
		{
			Department = department;
			UserId = userId;
		}

		public Department Department { get; }
		public Guid UserId { get; }
	}

	public sealed class DepartmentPrimaryManagerChangedEvent : DepartmentEvents
	{
		public DepartmentPrimaryManagerChangedEvent(
			Department department,
			Guid? oldPrimaryUserId,
			Guid newPrimaryUserId)
			: base(department.Id)
		{
			Department = department;
			OldPrimaryUserId = oldPrimaryUserId;
			NewPrimaryUserId = newPrimaryUserId;
		}

		public Department Department { get; }
		// Có thể null nếu trước đó chưa có primary.
		public Guid? OldPrimaryUserId { get; }
		public Guid NewPrimaryUserId { get; }
	}
}
