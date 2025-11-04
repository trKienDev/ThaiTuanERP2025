using MediatR;

namespace ThaiTuanERP2025.Application.Account.Users.Queries.ManagerIds
{
	public sealed record GetUserManagerIdsQuery(Guid UserId) : IRequest<IReadOnlyList<Guid>>;
}
