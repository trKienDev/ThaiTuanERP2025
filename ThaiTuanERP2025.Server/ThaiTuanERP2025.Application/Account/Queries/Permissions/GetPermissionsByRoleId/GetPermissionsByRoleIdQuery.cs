using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Queries.Permissions.GetPermissionsByRoleId
{
	public sealed record GetPermissionsByRoleIdQuery(Guid RoleId) : IRequest<IEnumerable<PermissionDto>>;
}
