using MediatR;

namespace ThaiTuanERP2025.Application.Account.Commands.Permissions.AssignPermissionToRole
{
	public sealed record AssignPermissionToRoleCommand(Guid RoleId, List<Guid> PermissionIds) : IRequest;
}
