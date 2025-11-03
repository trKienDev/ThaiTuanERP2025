using MediatR;

namespace ThaiTuanERP2025.Application.Account.Users.Commands.Create
{
	public sealed record CreateUserCommand(
		string Fullname,
		string Username,
		string EmployeeCode,
		string Password,
		Guid RoleId,
		string Position,
		Guid DepartmentId,
		string? Email,
		string? Phone
	) : IRequest<Unit>;
}
