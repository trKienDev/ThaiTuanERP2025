using MediatR;

namespace ThaiTuanERP2025.Application.Account.Departments.Queries.Parent
{
	public sealed record GetParentDepartmentQuery(Guid DeptId) : IRequest<DepartmentDto?>;
}
