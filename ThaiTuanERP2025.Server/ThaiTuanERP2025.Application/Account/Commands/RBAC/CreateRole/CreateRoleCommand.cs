using MediatR;

namespace ThaiTuanERP2025.Application.Account.Commands.RBAC.CreateRole
{
	public sealed record CreateRoleCommand (string Name, string Description) : IRequest<Guid>;
}
