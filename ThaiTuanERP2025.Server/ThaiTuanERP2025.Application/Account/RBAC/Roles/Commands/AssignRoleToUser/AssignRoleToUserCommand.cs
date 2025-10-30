using MediatR;

namespace ThaiTuanERP2025.Application.Account.RBAC.Roles.Commands.AssignRoleToUser
{
	public sealed record AssignRoleToUserCommand(Guid UserId, List<Guid> RoleIds) : IRequest<Unit>;
}
