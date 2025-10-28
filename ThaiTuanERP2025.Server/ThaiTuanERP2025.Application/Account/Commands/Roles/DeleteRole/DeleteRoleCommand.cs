using MediatR;

namespace ThaiTuanERP2025.Application.Account.Commands.Roles.DeleteRole
{
	public sealed record DeleteRoleCommand(Guid RoleId) : IRequest<Unit>;
}
