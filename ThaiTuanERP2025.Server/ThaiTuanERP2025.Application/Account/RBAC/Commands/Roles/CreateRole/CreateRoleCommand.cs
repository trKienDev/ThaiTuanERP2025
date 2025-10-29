using MediatR;
using ThaiTuanERP2025.Application.Account.RBAC.Requests;

namespace ThaiTuanERP2025.Application.Account.RBAC.Commands.Roles.CreateRole
{
	public sealed record CreateRoleCommand (RoleRequest Request) : IRequest<Unit>;
}
