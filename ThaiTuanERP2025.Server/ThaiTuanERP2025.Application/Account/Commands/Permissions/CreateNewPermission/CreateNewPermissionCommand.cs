using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Commands.Permissions.CreateNewPermission
{
	public sealed record CreateNewPermissionCommand(PermissionRequest request) : IRequest<Unit>;
}
