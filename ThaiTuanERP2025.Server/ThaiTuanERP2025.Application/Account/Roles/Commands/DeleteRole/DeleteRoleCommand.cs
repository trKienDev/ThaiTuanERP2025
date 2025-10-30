using MediatR;

namespace ThaiTuanERP2025.Application.Account.Roles.Commands.DeleteRole
{
	public sealed record DeleteRoleCommand(Guid RoleId) : IRequest<Unit>;
}
