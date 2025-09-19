using MediatR;

namespace ThaiTuanERP2025.Application.Account.Queries.Users.GetUserManagerIds
{
	public sealed record GetUserManagerIdsQuery(Guid UserId) : IRequest<List<Guid>>;
}
