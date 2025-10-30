using MediatR;
using ThaiTuanERP2025.Application.Account.Roles;

namespace ThaiTuanERP2025.Application.Account.Roles.Queries.GetAllRoles
{
	public sealed record GetAllRolesQuery : IRequest<IEnumerable<RoleDto>>;
}
