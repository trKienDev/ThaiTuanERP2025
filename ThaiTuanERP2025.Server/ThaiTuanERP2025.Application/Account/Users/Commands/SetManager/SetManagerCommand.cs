using MediatR;

namespace ThaiTuanERP2025.Application.Account.Users.Commands.SetManager
{
	public sealed record SetManagerCommand (
		Guid UserId,  IReadOnlyList<Guid> ManagerIds, Guid? PrimaryManagerId
	) : IRequest<Unit>;
}
