using MediatR;
using ThaiTuanERP2025.Domain.Account.Enums;

namespace ThaiTuanERP2025.Application.Account.Commands.Departments.AddDepartment
{
	public record AddDepartmentCommand(
		string Name, 
		string Code, 
		Region Region,
		Guid? ManagerId = null,
		Guid? ParentId = null
	) : IRequest<Unit>;
}
