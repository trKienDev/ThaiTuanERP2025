using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

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

	public sealed class DepartmentManagerChangedEvent : DepartmentEvents
	{
		public DepartmentManagerChangedEvent(Department department, Guid? managerUserId) : base(department.Id)
		{
			Department = department;
			ManagerUserId = managerUserId;
		}
		public Department Department { get; }
		public Guid? ManagerUserId { get; }
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

}
