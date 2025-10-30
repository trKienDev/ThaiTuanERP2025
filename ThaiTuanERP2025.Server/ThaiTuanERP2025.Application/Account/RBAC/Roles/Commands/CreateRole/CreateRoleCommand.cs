using MediatR;

namespace ThaiTuanERP2025.Application.Account.RBAC.Roles.Commands.CreateRole
{
	public sealed record CreateRoleCommand(RoleRequest Request) : IRequest<Unit>;
}
