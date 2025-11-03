using MediatR;

namespace ThaiTuanERP2025.Application.Account.Departments.Commands.SetParent
{
	public sealed record SetParentDepartmentCommand(Guid DeptId, Guid ParentId) : IRequest<Unit>;
}
