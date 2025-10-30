using MediatR;
using ThaiTuanERP2025.Application.Account.Departments;

namespace ThaiTuanERP2025.Application.Account.Queries.Departments.GetAllDepartments
{
	public record GetAllDepartmentsQuery : IRequest<List<DepartmentDto>>;
}
