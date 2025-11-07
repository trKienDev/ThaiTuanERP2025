using ThaiTuanERP2025.Application.Account.Users;

namespace ThaiTuanERP2025.Application.Account.Departments
{
	public record DepartmentBriefDto (
		Guid Id, 
		string Name, 
		string Code
	);

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
