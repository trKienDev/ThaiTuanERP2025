using ThaiTuanERP2025.Application.Account.Departments;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Roles;

namespace ThaiTuanERP2025.Application.Account.Users
{
	public sealed record UserDto 
	{
		public Guid Id { get; set; }
		public string FullName { get; set; } = default!;
		public string Username { get; set; } = default!;
		public string EmployeeCode { get; set; } = default!;
		public string Position { get; set; } = default!;
		public string? Email { get; set; }
		public string? Phone { get; set; }

		public Guid? AvatarFileId { get; set; }
		public string? AvatarFileObjectKey { get; set; }

		public IReadOnlyCollection<RoleDto> Roles { get; set; } = Array.Empty<RoleDto>();
		public IReadOnlyCollection<UserDto> Managers { get; set; } = Array.Empty<UserDto>();

		public List<PermissionDto> Permissions { get; set; } = new();

		public Guid? DepartmentId { get; set; }
		public DepartmentBriefDto? Department { get; set; } = default!;
	}

	public sealed record UserBriefDto
	{
		public Guid Id { get; set; }
		public string FullName { get; set; } = default!;
		public string Username { get; set; } = default!;
		public string EmployeeCode { get; set; } = default!;
	}

	public sealed record UserBriefAvatarDto {
		public Guid Id { get; set; }
		public string FullName { get; set; } = default!;
		public string Username { get; set; } = default!;
		public string EmployeeCode { get; set; } = default!;
		public Guid? AvatarFileId { get; set; }
		public string? AvatarFileObjectKey { get; set; }
	}

	public sealed record UserInforDto {
		public Guid Id { get; set; }
		public string FullName { get; set; } = default!;
		public string Username { get; set; } = default!;
		public string EmployeeCode { get; set; } = default!;
		public string? DepartmentName { get; set; }
		public IReadOnlyCollection<string> RoleNames { get; set; } = Array.Empty<string>();	
		public IReadOnlyCollection<UserBriefAvatarDto> Managers { get; set; } = Array.Empty<UserBriefAvatarDto>();
		public Guid? AvatarFileId { get; set; }
		public string? AvatarFileObjectKey { get; set; }
	}
}