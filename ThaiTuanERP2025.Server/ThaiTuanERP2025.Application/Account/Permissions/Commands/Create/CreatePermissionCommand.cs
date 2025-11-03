using MediatR;

namespace ThaiTuanERP2025.Application.Account.Permissions.Commands.Create
{
	public sealed record CreatePermissionCommand(PermissionRequest Request) : IRequest<Unit>;
}
