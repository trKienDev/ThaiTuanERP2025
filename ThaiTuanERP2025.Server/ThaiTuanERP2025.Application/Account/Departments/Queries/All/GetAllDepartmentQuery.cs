using MediatR;

namespace ThaiTuanERP2025.Application.Account.Departments.Queries.All
{
	public record GetAllDepartmentsQuery : IRequest<IReadOnlyList<DepartmentDto>>;
}
