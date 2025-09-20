using MediatR;

namespace ThaiTuanERP2025.Application.Account.Commands.Departments.SetDepartmentManager
{
	public sealed record SetDepartmentManagerCommand
	(
		Guid DepartmentId,
		Guid ManagerId
	) : IRequest<Unit>;
}
