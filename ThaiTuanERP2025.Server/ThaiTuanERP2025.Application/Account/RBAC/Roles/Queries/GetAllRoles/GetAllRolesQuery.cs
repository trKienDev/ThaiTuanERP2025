using MediatR;

namespace ThaiTuanERP2025.Application.Account.RBAC.Roles.Queries.GetAllRoles
{
	public sealed record GetAllRolesQuery : IRequest<IEnumerable<RoleDto>>;
}
