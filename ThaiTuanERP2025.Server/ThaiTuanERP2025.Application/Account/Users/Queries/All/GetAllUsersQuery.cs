using MediatR;

namespace ThaiTuanERP2025.Application.Account.Users.Queries.All
{
	public sealed record GetAllUsersQuery(string? keyword, string? role, Guid? departmentId) : IRequest<IReadOnlyList<UserDto>>;
}
