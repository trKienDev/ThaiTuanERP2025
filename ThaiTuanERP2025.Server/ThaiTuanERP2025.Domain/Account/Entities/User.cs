using ThaiTuanERP2025.Domain.Account.Events.Users;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Common.Entities;
using ThaiTuanERP2025.Domain.Common.ValueObjects;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class User : AuditableEntity
	{
		private readonly List<UserRole> _userRoles = new();
		private readonly List<UserGroup> _userGroups = new();
		private readonly List<UserManagerAssignment> _managerAssignments = new();
		private readonly List<UserManagerAssignment> _directReportsAssignments = new();
		private readonly List<BankAccount> _bankAccounts = new();

		private User() { } // EF

		public User(
			string fullName, string username, string employeeCode, string passwordHash, string position,
			Guid? departmentId, Email? email = null, Phone? phone = null, Guid? avatarFileId = null
		){
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
			Email = email;
			Phone = phone;
			AvatarFileId = avatarFileId;
			IsActive = true;
			IsSuperAdmin = false;

			AddDomainEvent(new UserCreatedEvent(this));
		}

		public string FullName { get; private set; } = string.Empty;
		public string Username { get; private set; } = string.Empty;
		public string EmployeeCode { get; private set; } = string.Empty;
		public string PasswordHash { get; private set; } = string.Empty;
		public Guid? AvatarFileId { get; private set; }
		public string Position { get; private set; } = string.Empty;

		public Email? Email { get; private set; }
		public Phone? Phone { get; private set; }

		public Guid? DepartmentId { get; private set; }
		public Department? Department { get; private set; }

		public Guid? ManagerId { get; private set; }
		public User? Manager { get; private set; }

		public bool IsSuperAdmin { get; private set; }
		public bool IsActive { get; private set; }

		public object? AvatarFileObjectKey;

		public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();
		public IReadOnlyCollection<UserGroup> UserGroups => _userGroups.AsReadOnly();
		public IReadOnlyCollection<UserManagerAssignment> ManagerAssignments => _managerAssignments.AsReadOnly();
		public IReadOnlyCollection<UserManagerAssignment> DirectReportsAssignments => _directReportsAssignments.AsReadOnly();
		public IReadOnlyCollection<BankAccount> BankAccounts => _bankAccounts.AsReadOnly();

		#region Domain Behaviors
		public void AssignManager(Guid managerId)
		{
			Guard.AgainstDefault(managerId, nameof(managerId));
			if (managerId == Id)
				throw new DomainException("Không thể tự làm quản lý chính mình");

			ManagerId = managerId;
			AddDomainEvent(new UserManagerAssignedEvent(this, managerId));
		}

		public void Activate()
		{
			if (IsActive) return;
			IsActive = true;
			AddDomainEvent(new UserActivatedEvent(this));
		}

		public void Deactivate()
		{
			if (!IsActive) return;
			IsActive = false;
			AddDomainEvent(new UserDeactivatedEvent(this));
		}

		public void SetSuperAdmin(bool isSuper)
		{
			if (IsSuperAdmin == isSuper) return;
			IsSuperAdmin = isSuper;
			AddDomainEvent(new UserSuperAdminChangedEvent(this, isSuper));
		}

		public void ChangePassword(string newPasswordHash)
		{
			Guard.AgainstNullOrWhiteSpace(newPasswordHash, nameof(newPasswordHash));
			PasswordHash = newPasswordHash;
			AddDomainEvent(new UserPasswordChangedEvent(this));
		}

		public void UpdateProfile(string fullName, string position, Email? email = null, Phone? phone = null)
		{
			Guard.AgainstNullOrWhiteSpace(fullName, nameof(fullName));
			FullName = fullName.Trim();
			Position = position.Trim();
			Email = email;
			Phone = phone;
			AddDomainEvent(new UserProfileUpdatedEvent(this));
		}

		public void UpdateAvatar(Guid? fileId)
		{
			AvatarFileId = fileId;
			AddDomainEvent(new UserAvatarUpdatedEvent(this));
		}

		public void AssignRole(Guid roleId)
		{
			if (_userRoles.Any(r => r.RoleId == roleId)) return;
			_userRoles.Add(new UserRole(Id, roleId));
			AddDomainEvent(new UserAssignedRoleEvent(this, roleId));
		}

		public void RemoveRole(Guid roleId)
		{
			var existing = _userRoles.FirstOrDefault(r => r.RoleId == roleId);
			if (existing == null) return;

			_userRoles.Remove(existing);
			AddDomainEvent(new UserRemovedRoleEvent(this, roleId));
		}

		public void SetDepartment(Guid departmentId)
		{
			Guard.AgainstDefault(departmentId, nameof(departmentId));
			DepartmentId = departmentId;
			AddDomainEvent(new UserDepartmentChangedEvent(this, departmentId));
		}
		#endregion
	}
}