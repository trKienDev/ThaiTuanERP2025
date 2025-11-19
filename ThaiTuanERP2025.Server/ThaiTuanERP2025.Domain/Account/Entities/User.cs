using ThaiTuanERP2025.Domain.Account.Events;
using ThaiTuanERP2025.Domain.Account.ValueObjects;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Files.Entities;
using ThaiTuanERP2025.Domain.Shared.Interfaces;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class User : BaseEntity, IActiveEntity
	{
		#region EF Constructor
		private User() { }
		public User(
			string fullName, string username, string employeeCode, string passwordHash, string position,
			Guid? departmentId, string? email = null, string? phone = null, Guid? avatarFileId = null
		)
		{
			Guard.AgainstNullOrWhiteSpace(fullName, nameof(fullName));
			Guard.AgainstNullOrWhiteSpace(username, nameof(username));
			Guard.AgainstNullOrWhiteSpace(passwordHash, nameof(passwordHash));

			Id = Guid.NewGuid();
			FullName = fullName.Trim();
			Username = username.Trim();
			EmployeeCode = employeeCode.Trim();
			PasswordHash = passwordHash;
			Position = position.Trim();
			DepartmentId = departmentId;
			Email = string.IsNullOrWhiteSpace(email) ? null : new Email(email);
			Phone = string.IsNullOrWhiteSpace(phone) ? null : new Phone(phone);
			AvatarFileId = avatarFileId;
			IsActive = true;
			IsSuperAdmin = false;

			AddDomainEvent(new UserCreatedEvent(this));
		}
		#endregion

		private readonly List<UserRole> _userRoles = new();
		private readonly List<UserGroup> _userGroups = new();
		private readonly List<UserManagerAssignment> _managerAssignments = new();
		private readonly List<UserManagerAssignment> _directReportsAssignments = new();

		#region Properties
		public string FullName { get; private set; } = string.Empty;
		public string Username { get; private set; } = string.Empty;
		public string EmployeeCode { get; private set; } = string.Empty;
		public string PasswordHash { get; private set; } = string.Empty;

		public Guid? AvatarFileId { get; private set; }
		public string? AvatarFileObjectKey { get; private set; }
		public StoredFile? AvatarFile { get; init; }

		public string Position { get; private set; } = string.Empty;

		public Email? Email { get; private set; }
		public Phone? Phone { get; private set; }

		public Guid? DepartmentId { get; private set; }
		public Department? Department { get; init; }

		public Guid? ManagerId { get; private set; }
		public User? Manager { get; init; }

		public bool IsSuperAdmin { get; private set; }
		public bool IsActive { get; private set; } = true;

		public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();
		public IReadOnlyCollection<UserGroup> UserGroups => _userGroups.AsReadOnly();
		public IReadOnlyCollection<UserManagerAssignment> ManagerAssignments => _managerAssignments.AsReadOnly();
		public IReadOnlyCollection<UserManagerAssignment> DirectReportsAssignments => _directReportsAssignments.AsReadOnly();
		#endregion

		

		#region Domain Behaviors
		internal void AssignManager(Guid managerId)
		{
			Guard.AgainstDefault(managerId, nameof(managerId));
			if (managerId == Id)
				throw new DomainException("Không thể tự làm quản lý chính mình");

			ManagerId = managerId;
			AddDomainEvent(new UserManagerAssignedEvent(this, managerId));
		}

		internal void Activate()
		{
			if (IsActive) return;
			IsActive = true;
			AddDomainEvent(new UserActivatedEvent(this));
		}

		internal void Deactivate()
		{
			if (!IsActive) return;
			IsActive = false;
			AddDomainEvent(new UserDeactivatedEvent(this));
		}

		internal void SetSuperAdmin(bool isSuper)
		{
			if (IsSuperAdmin == isSuper) return;
			IsSuperAdmin = isSuper;
			AddDomainEvent(new UserSuperAdminChangedEvent(this, isSuper));
		}

		internal void ChangePassword(string newPasswordHash)
		{
			Guard.AgainstNullOrWhiteSpace(newPasswordHash, nameof(newPasswordHash));
			PasswordHash = newPasswordHash;
			AddDomainEvent(new UserPasswordChangedEvent(this));
		}

		internal void UpdateProfile(string fullName, string position, Email? email = null, Phone? phone = null)
		{
			Guard.AgainstNullOrWhiteSpace(fullName, nameof(fullName));
			FullName = fullName.Trim();
			Position = position.Trim();
			Email = email;
			Phone = phone;
			AddDomainEvent(new UserProfileUpdatedEvent(this));
		}

		internal void UpdateAvatar (Guid fileId, string objectKey)
		{
			AvatarFileId = fileId;
			AvatarFileObjectKey = objectKey;

			AddDomainEvent(new UserAvatarUpdatedEvent(this));
		}

		internal void AssignRole(Guid roleId)
		{
			if (_userRoles.Any(r => r.RoleId == roleId)) return;
			_userRoles.Add(new UserRole(Id, roleId));
			AddDomainEvent(new UserAssignedRoleEvent(this, roleId));
		}

		internal void RemoveRole(Guid roleId)
		{
			var existing = _userRoles.FirstOrDefault(r => r.RoleId == roleId);
			if (existing == null) return;

			_userRoles.Remove(existing);
			AddDomainEvent(new UserRemovedRoleEvent(this, roleId));
		}

		internal void SetDepartment(Guid departmentId)
		{
			Guard.AgainstDefault(departmentId, nameof(departmentId));
			DepartmentId = departmentId;
			AddDomainEvent(new UserDepartmentChangedEvent(this, departmentId));
		}

		internal void DeletePermanently()
		{
			// 1) Chặn các trường hợp không hợp lệ
			if (IsSuperAdmin)
				throw new DomainException("Không thể xóa vĩnh viễn tài khoản Super Admin.");

			// Nếu còn người báo cáo trực tiếp → buộc chuyển/huỷ quản lý trước khi xóa
			if (_directReportsAssignments.Any())
				throw new DomainException("Không thể xóa vì user còn có nhân viên đang báo cáo trực tiếp.");

			// 2) Dọn các quan hệ con thuộc aggregate User
			_userRoles.Clear();
			_userGroups.Clear();
			_managerAssignments.Clear();
			_directReportsAssignments.Clear();

			// Dọn các reference tùy chọn để tránh FK còn tham chiếu
			ManagerId = null;
			DepartmentId = null;
			AvatarFileId = null;

			// 3) Phát event báo “xóa vĩnh viễn”
			AddDomainEvent(new UserPermanentDeleteRequestedEvent(this));
		}
		#endregion
	}
}