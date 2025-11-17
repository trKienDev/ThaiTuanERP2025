using ThaiTuanERP2025.Domain.Account.Events;
using ThaiTuanERP2025.Domain.Account.Specifications;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class Role : BaseEntity
	{
		private readonly List<RolePermission> _rolePermissions = new();
		private readonly List<UserRole> _userRoles = new();

		#region Properties
		public string Name { get; private set; } = default!;
		public string Description { get; private set; } = string.Empty;
		public bool IsActive { get; private set; } = true;

		public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();
		public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();
		#endregion

		#region EF Constructor
		private Role() { } 
		public Role(string name, string description = "")
		{
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Name = name;
			Description = description;
			IsActive = true;

			AddDomainEvent(new RoleCreatedEvent(this));
		}
		#endregion

		#region Domain Behaviors
		internal void Activate()
		{
			if (IsActive) return;
			IsActive = true;
			AddDomainEvent(new RoleActivatedEvent(this));
		}
		internal void Deactivate()
		{
			if (!IsActive) return;
			IsActive = false;
			AddDomainEvent(new RoleDeactivatedEvent(this));
		}

		internal void Update(string name, string description)
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Description = description;
			AddDomainEvent(new RoleUpdatedEvent(this));
		}

		/// <summary>
		/// Gán một permission cho Role. Chỉ Role đang active mới được gán.
		/// </summary>
		internal void AssignPermission(Guid permissionId)
		{
			// Sử dụng Specification để kiểm tra điều kiện domain
			var activeSpec = new ActiveRoleLinqSpec();
			if (!activeSpec.IsSatisfiedBy(this))
				throw new InvalidEntityStateException("Không thể gán quyền cho Role không hoạt động.");

			if (_rolePermissions.Any(rp => rp.PermissionId == permissionId))
				return;

			_rolePermissions.Add(new RolePermission(Id, permissionId));
			AddDomainEvent(new PermissionAssignedToRoleEvent(this, permissionId));
		}

		internal void RemovePermission(Guid permissionId)
		{
			var existing = _rolePermissions.FirstOrDefault(rp => rp.PermissionId == permissionId);
			if (existing == null) return;

			_rolePermissions.Remove(existing);
			AddDomainEvent(new PermissionRemovedFromRoleEvent(this, permissionId));
		}

		internal void AssignUser(Guid userId)
		{
			if (_userRoles.Any(ur => ur.UserId == userId))
				return;

			_userRoles.Add(new UserRole(userId, Id));
			AddDomainEvent(new UserAssignedToRoleEvent(this, userId));
		}

		internal void RemoveUser(Guid userId)
		{
			var existing = _userRoles.FirstOrDefault(ur => ur.UserId == userId);
			if (existing == null) return;

			_userRoles.Remove(existing);
			AddDomainEvent(new UserRemovedFromRoleEvent(this, userId));
		}

		/// Kiểm tra Role có permission cụ thể không.
		public bool HasPermission(Guid permissionId)
		{
			var spec = new RoleHasPermissionLinqSpec(permissionId);
			return spec.IsSatisfiedBy(this);
		}
		#endregion
	}
}
