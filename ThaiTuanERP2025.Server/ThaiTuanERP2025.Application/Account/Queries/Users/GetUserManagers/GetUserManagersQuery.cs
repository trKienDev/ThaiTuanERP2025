using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Queries.Users.GetUserManagers
{
	public sealed record GetUserManagersQuery(Guid UserId) : IRequest<List<UserDto>>;
}
