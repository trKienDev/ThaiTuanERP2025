using ThaiTuanERP2025.Application.Account.Users;

namespace ThaiTuanERP2025.Application.Account.Departments
{
	public record DepartmentBriefDto {
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Code { get; set; } = string.Empty;
	};

	public record DepartmentDto
	(
		Guid Id,
		string Name,
		string Code,
		Guid? ParentId,
		UserDto? PrimaryManager,
		IReadOnlyList<UserDto> ViceManagers
	);
}
