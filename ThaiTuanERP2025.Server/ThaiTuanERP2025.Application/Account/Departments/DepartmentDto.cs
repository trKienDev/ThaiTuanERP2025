using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Domain.Account.Enums;

namespace ThaiTuanERP2025.Application.Account.Departments
{
	public record DepartmentDto
	{
		public Guid Id { get; init; }
		public string Name { get; init; } = string.Empty;
		public string Code { get; init; } = string.Empty;
		public Region Region { get; init; }
		public Guid? ParentId { get; init; }

		public Guid? ManagerUserId { get; init; }
		public UserDto? Manager { get; init; }
	};
}
