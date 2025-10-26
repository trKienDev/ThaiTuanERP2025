using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Queries.Permissions.GetAllPermissions
{
	public sealed record GetAllPermissionsQuery : IRequest<IEnumerable<PermissionDto>>;
}
