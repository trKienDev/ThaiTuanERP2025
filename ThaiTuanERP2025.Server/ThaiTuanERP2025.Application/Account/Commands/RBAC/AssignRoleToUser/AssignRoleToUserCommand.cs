using MediatR;

namespace ThaiTuanERP2025.Application.Account.Commands.RBAC.AssignRoleToUser
{
	public sealed record AssignRoleToUserCommand(Guid UserId, Guid RoleId) : IRequest<Unit>;
}
