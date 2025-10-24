using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class User
	{
		public object? AvatarFileObjectKey;

		public Guid Id { get; set; }
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

		public ICollection<UserGroup> UserGroups { get; private set; }
		public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();

		public bool IsSuperAdmin { get; private set; } = false;
		public bool IsActive { get; private set; } = true;

		public ICollection<BankAccount> BankAccounts { get; private set; } = new List<BankAccount>();

		// ManagerAssignments: các quan hệ mà User là Manager của người khác (direct reports).
		public ICollection<UserManagerAssignment> ManagerAssignments { get; private set; } = new List<UserManagerAssignment>();
		// DirectReportsAssignments: các quan hệ mà User là nhân viên và có các manager khác nhau.
		public ICollection<UserManagerAssignment> DirectReportsAssignments { get; private set; } = new List<UserManagerAssignment>();

		// EF Core cần constructor mặc định
		private User() {
			UserGroups = new List<UserGroup>();
		}

		public User(
			string fullName, 
			string userName, 
			string employeeCode,
			string passwordHash,
			string position,
			Guid? departmentId,
			Email? email = null,
			Phone? phone = null,
			Guid? avatarFileId = null
		) {
			if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Tên không được để trống");
			if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentException("Username không hợp lệ");
			if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("Password không hợp lệ");

			Id = Guid.NewGuid();
			FullName = fullName;	
			Username = userName;
			EmployeeCode = employeeCode;
			PasswordHash = passwordHash;
			Email = email;
			Phone = phone;
			AvatarFileId = avatarFileId;
			Position = position;
			DepartmentId = departmentId;
			UserGroups = new List<UserGroup>();
			IsActive = true;
			IsSuperAdmin = false;
		}

		// Domain methods
		public void AssignManager(Guid managerId) {
			if (managerId == Id) throw new InvalidOperationException("Không thể tự làm quản lý chính mình");
			ManagerId = managerId;
		}
		public void SetSuperAdmin(bool isSuper) => IsSuperAdmin = isSuper;
		public void Activate() => IsActive = true;
		public void Deactivate() => IsActive = false;
		
		public void ChangePassword(string newPasswordHash) {
			if (string.IsNullOrWhiteSpace(newPasswordHash)) throw new ArgumentException("Mật khẩu mới không hợp lệ");
			PasswordHash = newPasswordHash;
		}

		public void UpdateProfile(
			string fullName,
			string avatarUrl,
			string position,
			Email? email = null,
			Phone? phone = null
		)
		{
			if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Tên không được để trống");
			FullName = fullName;
			Position = position;
			Email = email;	
			Phone = phone;	
		}

		public void SetDepartment(Guid departmentId) =>DepartmentId = departmentId;
		public void UpdateAvatar(Guid? fileId) => AvatarFileId = fileId;

		public void AssignRole(Guid roleId)
		{
			if (UserRoles.Any(r => r.RoleId == roleId)) return;
			UserRoles.Add(new UserRole(Id, roleId));
		}

		public void RemoveRole(Guid roleId)
		{
			var role = UserRoles.FirstOrDefault(r => r.RoleId == roleId);
			if (role != null)
				UserRoles.Remove(role);
		}
	}
}