using MediatR;

namespace ThaiTuanERP2025.Application.Account.RBAC.Roles.Commands.DeleteRole
{
	public sealed record DeleteRoleCommand(Guid RoleId) : IRequest<Unit>;
}
