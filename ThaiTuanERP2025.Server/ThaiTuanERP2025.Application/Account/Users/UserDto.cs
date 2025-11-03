using ThaiTuanERP2025.Application.Account.Departments;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Roles;

namespace ThaiTuanERP2025.Application.Account.Users
{
	public class UserDto
	{
		public Guid Id { get; set; }
		public string FullName { get; set; } = default!;
		public string Username { get; set; } = default!;
		public string EmployeeCode { get; set; } = default!;
		public string Position { get; set; } = default!;
		public string? Email { get; set; }
		public string? Phone { get; set; }

		public Guid? AvatarFileId { get; set; }
		public object? AvatarFileObjectKey { get; set; }

		public IReadOnlyCollection<RoleDto> Roles { get; set; } = Array.Empty<RoleDto>();
		public IReadOnlyCollection<UserDto> Managers { get; set; } = Array.Empty<UserDto>();

		public List<PermissionDto> Permissions { get; set; } = new();

		public Guid? DepartmentId { get; set; }
		public DepartmentDto? Department { get; set; } = default!;
	}

	
}