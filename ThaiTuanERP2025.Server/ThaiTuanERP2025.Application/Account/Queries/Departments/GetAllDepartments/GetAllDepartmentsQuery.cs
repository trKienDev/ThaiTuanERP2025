using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Queries.Departments.GetAllDepartments
{
	public record GetAllDepartmentsQuery : IRequest<List<DepartmentDto>>;
}
