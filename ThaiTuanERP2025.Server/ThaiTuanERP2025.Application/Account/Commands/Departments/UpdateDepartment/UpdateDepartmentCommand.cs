using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Account.Commands.Departments.UpdateDepartment
{
    public record UpdateDepartmentCommand(Guid Id, string Code, string Name) : IRequest;
}
