using ThaiTuanERP2025.Domain.Account.Events;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Common.Entities;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class Department : AuditableEntity
	{
		private readonly List<User> _users = new();
		private readonly List<Department> _children = new();
		private readonly List<DepartmentManager> _managers = new();

		#region Properties
		public string Name { get; private set; } = string.Empty;
		public string Code { get; private set; } = string.Empty;
		public bool IsActive { get; private set; }
		public int Level { get; private set; }

		public ICollection<User> Users => _users;
		public ICollection<Department> Children => _children;
		public ICollection<DepartmentManager> Managers => _managers;

		public Guid? ParentId { get; private set; }
		public Department? Parent { get; private set; }
		#endregion

		#region Constructor
		private Department() { }
		public Department(string name, string code)
		{
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Guard.AgainstNullOrWhiteSpace(code, nameof(code));

			Id = Guid.NewGuid();
			Name = name.Trim();
			Code = code.ToUpperInvariant();
			IsActive = true;

			AddDomainEvent(new DepartmentCreatedEvent(this));
		}
		#endregion

		#region Domain Behaviors
		public void Rename(string newName)
		{
			Guard.AgainstNullOrWhiteSpace(newName, nameof(newName));
			Name = newName.Trim();
			AddDomainEvent(new DepartmentRenamedEvent(this));
		}

		public void AssignManager(User user)
		{
			if (user is null) throw new ArgumentNullException(nameof(user));
			if (user.DepartmentId != Id)
				throw new DomainException("User phải thuộc chính Department này mới được gán làm manager.");

			if (_managers.Any(m => m.UserId == user.Id)) return;
			_managers.Add(new DepartmentManager(Id, user.Id, isPrimary: false));
			AddDomainEvent(new DepartmentManagerAddedEvent(this, user.Id));
		}

		public void RemoveManager(Guid userId)
		{
			var link = _managers.FirstOrDefault(m => m.UserId == userId);
			if (link == null) return;
			_managers.Remove(link);
			AddDomainEvent(new DepartmentManagerRemovedEvent(this, userId));
		}

		public void SetPrimaryManager(Guid userId)
		{
			if (_managers.All(m => m.UserId != userId))
				throw new DomainException("User chưa là manager của phòng ban.");

			// Nếu đã là primary rồi thì không làm gì (tránh bắn event thừa)
			var currentPrimary = _managers.FirstOrDefault(m => m.IsPrimary)?.UserId;
			if (currentPrimary.HasValue && currentPrimary.Value == userId) return;

			var oldPrimaryUserId = currentPrimary;

			foreach (var m in _managers) m.UnsetPrimary();
			_managers.First(m => m.UserId == userId).SetPrimary();

			AddDomainEvent(new DepartmentPrimaryManagerChangedEvent(this, oldPrimaryUserId, userId));
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

		public void SetParent(Department? newParent)
		{
			// 1) Không cho tự làm cha chính mình
			if (newParent != null && newParent.Id == this.Id)
				throw new InvalidOperationException("Không thể đặt parent là chính nó.");

			// 2) Chặn vòng lặp: newParent không được là hậu duệ của this
			if (newParent != null && newParent.IsDescendantOf(this))
				throw new InvalidOperationException("Không thể đặt parent là một hậu duệ của phòng ban hiện tại.");

			// 3) Tháo liên kết khỏi parent cũ (nếu có)
			var oldParentId = this.ParentId;
			if (this.Parent != null)
			{
				// Bỏ this khỏi danh sách Children của parent cũ
				this.Parent._children.RemoveAll(c => c.Id == this.Id);
				// (RemoveAll là extension của List<T> .NET 8; nếu .NET thấp hơn, dùng Remove bằng cách tìm đối tượng)
			}

			// 4) Gán parent mới + đồng bộ 2 chiều
			this.Parent = newParent;
			this.ParentId = newParent?.Id;

			if (newParent != null && !newParent._children.Any(c => c.Id == this.Id))
				newParent._children.Add(this);

			// 5) Cập nhật level (root = 0, con = parent.Level + 1)
			var newLevel = newParent == null ? 0 : newParent.Level + 1;
			UpdateLevelRecursively(newLevel);

			AddDomainEvent(new DepartmentParentChangedEvent(this, oldParentId, this.ParentId));
		}

		// Hỗ trợ: kiểm tra newParent có phải hậu duệ của node hiện tại không
		private bool IsDescendantOf(Department potentialAncestor)
		{
			var current = this.Parent;
			while (current != null)
			{
				if (current.Id == potentialAncestor.Id) return true;
				current = current.Parent;
			}
			return false;
		}

		// Hỗ trợ: set level cho node hiện tại và toàn bộ cây con
		private void UpdateLevelRecursively(int level)
		{
			this.Level = level;
			foreach (var child in _children)
			{
				child.UpdateLevelRecursively(level + 1);
			}
		}
		#endregion
	}
}
