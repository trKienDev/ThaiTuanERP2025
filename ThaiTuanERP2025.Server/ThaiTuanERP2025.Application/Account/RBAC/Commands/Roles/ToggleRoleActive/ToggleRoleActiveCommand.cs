using MediatR;

namespace ThaiTuanERP2025.Application.Account.RBAC.Commands.Roles.ToggleRoleActive
{
	public sealed record ToggleRoleActiveCommand(Guid Id) : IRequest<Unit>;
}
