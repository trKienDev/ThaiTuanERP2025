using MediatR;

namespace ThaiTuanERP2025.Application.Account.Roles.Queries.GetAllRoles
{
	public sealed record GetAllRolesQuery : IRequest<IEnumerable<RoleDto>>;
}
