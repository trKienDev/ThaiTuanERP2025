using MediatR;

namespace ThaiTuanERP2025.Application.Account.Users.Queries
{
	public sealed record GetAllUsersQuery(string? keyword, string? role, Guid? departmentId) : IRequest<IReadOnlyList<UserDto>>;
	public sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IReadOnlyList<UserDto>>
	{
		private readonly IUserReadRepostiory _userReadRepo;
		public GetAllUsersQueryHandler(IUserReadRepostiory userReadRepo)
		{
			_userReadRepo = userReadRepo;
		}

		public async Task<IReadOnlyList<UserDto>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
		{
			return await _userReadRepo.ListUserDtosWithAvatarAsync(query.keyword?.Trim(), query.role?.Trim(), query.departmentId, cancellationToken);
		}
	}
}
