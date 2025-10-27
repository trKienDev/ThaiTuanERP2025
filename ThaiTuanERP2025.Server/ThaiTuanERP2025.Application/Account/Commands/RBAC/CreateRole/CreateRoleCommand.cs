using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Commands.RBAC.CreateRole
{
	public sealed record CreateRoleCommand (RoleRequest Request) : IRequest<Unit>;
}
