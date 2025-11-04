using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Permissions.Queries.ByRole
{
	public sealed record GetPermissionsByRoleIdQuery(Guid RoleId) : IRequest<IReadOnlyList<PermissionDto>>;
}
