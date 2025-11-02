using ThaiTuanERP2025.Application.Account.Users;

namespace ThaiTuanERP2025.Application.Account.Departments
{
	public record DepartmentDto
	{
		public Guid Id { get; init; }
		public string Name { get; init; } = string.Empty;
		public string Code { get; init; } = string.Empty;
		public Guid? ParentId { get; init; }

		public Guid? ManagerUserId { get; init; }
		public UserDto? Manager { get; init; }
	};
}
