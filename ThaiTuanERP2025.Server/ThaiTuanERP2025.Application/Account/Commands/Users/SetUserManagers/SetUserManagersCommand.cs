using MediatR;

namespace ThaiTuanERP2025.Application.Account.Commands.Users.SetUserManagers
{
	public sealed record SetUserManagersCommand(
		Guid UserId,
		IReadOnlyList<Guid> ManagerIds,
		Guid? PrimaryManagerId
	) : IRequest<Unit>;
}
