using MediatR;

namespace ThaiTuanERP2025.Application.Account.Permissions.Commands.AssignRole
{
	public sealed record AssignPermissionToRoleCommand(Guid RoleId, List<Guid> PermissionIds) : IRequest<Unit>;
}
