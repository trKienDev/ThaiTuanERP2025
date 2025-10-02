using MediatR;

namespace ThaiTuanERP2025.Application.Account.Commands.Departments.UpdateDepartment
{
    public record UpdateDepartmentCommand(Guid Id, string Code, string Name) : IRequest;
}
