using MediatR;

namespace ThaiTuanERP2025.Application.Account.Departments.Commands.Create
{
	public sealed record CreateDepartmentCommand(
		string Name,
		string Code,
		Guid? ManagerId = null,
		Guid? ParentId = null
	) : IRequest<Unit>;
}
