using MediatR;

namespace ThaiTuanERP2025.Application.Account.Commands.RBAC.AssignPermissionToRole
{
	public sealed record AssignPermissionToRoleCommand(Guid RoleId, Guid PermissionId) : IRequest;
}
