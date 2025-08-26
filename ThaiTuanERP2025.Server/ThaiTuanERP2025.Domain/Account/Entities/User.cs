using System;
using System.Security.Cryptography.X509Certificates;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Account.Enums;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class User
	{
		public Guid Id { get; set; }
		public string FullName { get; private set; } = string.Empty;
		public string Username { get; private set; } = string.Empty;
		public string EmployeeCode { get; private set; } = string.Empty;
		public string PasswordHash { get; private set; } = string.Empty;
		public Guid? AvatarFileId { get; private set; }
		public UserRole Role { get; private set; }
		public string Position { get; private set; } = string.Empty;
		
		public Email? Email { get; private set; }
		public Phone? Phone { get; private set; }

		public Guid? DepartmentId { get; private set; }
		public Department? Department { get; private set; }

		public Guid? ManagerId { get; private set; }
		public User? Manager { get; private set; }

		public ICollection<UserGroup> UserGroups { get; private set; }

		public bool IsSuperAdmin { get; private set; } = false;
		public bool IsActive { get; private set; } = true;

		// EF Core cần constructor mặc định
		private User() {
			UserGroups = new List<UserGroup>();
		}

		public User(
			string fullName, 
			string userName, 
			string employeeCode,
			string passwordHash,
			UserRole role,
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
			Role = role;
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
		public bool HasRole(UserRole role) => Role == role;
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

		public void SetRole(UserRole role) => Role = role;
		public void SetDepartment(Guid departmentId) =>DepartmentId = departmentId;
		public void UpdateAvatar(Guid? fileId) => AvatarFileId = fileId;

	}
}