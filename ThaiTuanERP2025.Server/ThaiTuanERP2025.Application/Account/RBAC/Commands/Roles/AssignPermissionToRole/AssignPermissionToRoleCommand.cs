using MediatR;

namespace ThaiTuanERP2025.Application.Account.RBAC.Commands.Roles.AssignPermissionToRole
{
	public sealed record AssignPermissionToRoleCommand(Guid RoleId, List<Guid> PermissionIds) : IRequest<Unit>;
}
