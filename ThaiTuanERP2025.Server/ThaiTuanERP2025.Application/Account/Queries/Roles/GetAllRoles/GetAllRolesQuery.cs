using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Queries.Roles.GetAllRoles
{
	public sealed record GetAllRolesQuery : IRequest<IEnumerable<RoleDto>>;
}
