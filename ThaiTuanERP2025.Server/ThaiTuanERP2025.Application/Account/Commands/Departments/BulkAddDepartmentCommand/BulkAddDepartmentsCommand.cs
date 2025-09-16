using MediatR;
using ThaiTuanERP2025.Domain.Account.Enums;

namespace ThaiTuanERP2025.Application.Account.Commands.Departments.BulkAddDepartmentCommand
{
	public record DepartmentDtoForImport(string Code, string Name, Region Region, Guid? DivisionId = null, Guid? ManagerId = null);
	public record BulkAddDepartmentsCommand(List<DepartmentDtoForImport> Departments) : IRequest<Unit>;
}
