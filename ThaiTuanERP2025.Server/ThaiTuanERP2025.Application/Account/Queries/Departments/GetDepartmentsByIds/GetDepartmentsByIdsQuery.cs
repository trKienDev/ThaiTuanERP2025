using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Queries.Departments.GetDepartmentsByIds
{
	public record GetDepartmentsByIdsQuery(IEnumerable<Guid> DepartmentIds) : IRequest<List<DepartmentDto>>;
}
