using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Account.Commands.AddDepartment
{
	public record DepartmentDtoForImport(string Code, string Name);
	public record BulkAddDepartmentsCommand(List<DepartmentDtoForImport> Departments) : IRequest<Unit>;
}
