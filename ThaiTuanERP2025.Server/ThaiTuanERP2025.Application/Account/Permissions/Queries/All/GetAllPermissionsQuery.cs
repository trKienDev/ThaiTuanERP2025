using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Permissions.Queries.All
{
	public sealed record GetAllPermissionsQuery : IRequest<IReadOnlyList<PermissionDto>>;
}
