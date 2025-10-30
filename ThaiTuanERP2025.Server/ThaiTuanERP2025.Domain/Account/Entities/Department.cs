using ThaiTuanERP2025.Domain.Account.Enums;
using ThaiTuanERP2025.Domain.Account.Events.Departments;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class Department : AuditableEntity
	{
		private readonly List<User> _users = new();
		private readonly List<Department> _children = new();

		private Department() { }
		public Department(string name, string code, Region region, Guid? managerUserId = null)
		{
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Guard.AgainstNullOrWhiteSpace(code, nameof(code));

			Id = Guid.NewGuid();
			Name = name.Trim();
			Code = code.ToUpperInvariant();
			Region = region;
			ManagerUserId = managerUserId;
			IsActive = true;

			AddDomainEvent(new DepartmentCreatedEvent(this));
		}

		public string Name { get; private set; } = string.Empty;
		public string Code { get; private set; } = string.Empty;
		public bool IsActive { get; private set; }
		public int Level { get; private set; }
		public Region Region { get; private set; }
		public Guid? ManagerUserId { get; private set; }
		public Guid? ParentId { get; private set; }
		public Department? Parent { get; private set; }

		public IReadOnlyCollection<User> Users => _users.AsReadOnly();
		public IReadOnlyCollection<Department> Children => _children.AsReadOnly();

		#region Domain Behaviors
		public void Rename(string newName)
		{
			Guard.AgainstNullOrWhiteSpace(newName, nameof(newName));
			Name = newName.Trim();
			AddDomainEvent(new DepartmentRenamedEvent(this));
		}

		public void AssignManager(Guid? userId)
		{
			if (userId.HasValue)
				Guard.AgainstDefault(userId.Value, nameof(userId));
			ManagerUserId = userId;
			AddDomainEvent(new DepartmentManagerChangedEvent(this, userId));
		}

		public void Activate()
		{
			if (IsActive) return;
			IsActive = true;
			AddDomainEvent(new DepartmentActivatedEvent(this));
		}

		public void Deactivate()
		{
			if (!IsActive) return;
			IsActive = false;
			AddDomainEvent(new DepartmentDeactivatedEvent(this));
		}

		public void AddChild(Department child)
		{
			if (child == null) throw new ArgumentNullException(nameof(child));
			if (_children.Any(c => c.Id == child.Id))
				throw new InvalidOperationException("Phòng ban con đã tồn tại.");

			_children.Add(child);
			child.ParentId = Id;
			AddDomainEvent(new DepartmentChildAddedEvent(this, child));
		}

		public void RemoveChild(Guid childId)
		{
			var child = _children.FirstOrDefault(c => c.Id == childId);
			if (child == null) return;

			_children.Remove(child);
			AddDomainEvent(new DepartmentChildRemovedEvent(this, child));
		}
		#endregion
	}
}
