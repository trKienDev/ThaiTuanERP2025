using MediatR;
using ThaiTuanERP2025.Application.Account.Roles;

namespace ThaiTuanERP2025.Application.Account.Roles.Commands.CreateRole
{
	public sealed record CreateRoleCommand(RoleRequest Request) : IRequest<Unit>;
}
