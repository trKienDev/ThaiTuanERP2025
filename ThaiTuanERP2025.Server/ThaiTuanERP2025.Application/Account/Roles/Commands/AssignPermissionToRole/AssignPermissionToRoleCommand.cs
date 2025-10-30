using MediatR;

namespace ThaiTuanERP2025.Application.Account.Roles.Commands.AssignPermissionToRole
{
	public sealed record AssignPermissionToRoleCommand(Guid RoleId, List<Guid> PermissionIds) : IRequest<Unit>;
}
