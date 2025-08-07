using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Account.Commands.Departments.DeleteDepartment
{
    public record DeleteDepartmentCommand(Guid id) : IRequest;
}
