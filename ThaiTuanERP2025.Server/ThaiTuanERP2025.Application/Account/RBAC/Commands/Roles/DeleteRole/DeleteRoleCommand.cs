using MediatR;

namespace ThaiTuanERP2025.Application.Account.RBAC.Commands.Roles.DeleteRole
{
	public sealed record DeleteRoleCommand(Guid RoleId) : IRequest<Unit>;
}
