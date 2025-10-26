using MediatR;

namespace ThaiTuanERP2025.Application.Account.Commands.Roles.AssignRoleToUser
{
	public sealed record AssignRoleToUserCommand(Guid UserId, List<Guid> RoleIds) : IRequest<Unit>;
}
