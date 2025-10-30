using MediatR;
using ThaiTuanERP2025.Application.Account.RBAC.Dtos;

namespace ThaiTuanERP2025.Application.Account.RBAC.Queries.Roles.GetAllRoles
{
	public sealed record GetAllRolesQuery : IRequest<IEnumerable<RoleDto>>;
}
