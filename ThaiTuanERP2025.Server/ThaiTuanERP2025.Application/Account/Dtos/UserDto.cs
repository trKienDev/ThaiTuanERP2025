namespace ThaiTuanERP2025.Application.Account.Dtos
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

		public string? AvatarFileId { get; set; }
		public string? AvatarFileObjectKey { get; set; }

		public List<RoleDto> Roles { get; set; } = new();
		public List<PermissionDto> Permissions { get; set; } = new();

		public Guid? DepartmentId { get; set; }

		public DepartmentDto? Department { get; set; } = default!;
	}

	
}