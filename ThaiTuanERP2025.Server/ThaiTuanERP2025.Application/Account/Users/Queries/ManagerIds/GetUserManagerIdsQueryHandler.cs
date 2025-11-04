using MediatR;

namespace ThaiTuanERP2025.Application.Account.Users.Queries.ManagerIds
{
	public class GetUserManagerIdsQueryHandler : IRequestHandler<GetUserManagerIdsQuery, IReadOnlyList<Guid>>
	{
		private readonly IUserReadRepostiory _userReadRepo;
		public GetUserManagerIdsQueryHandler(IUserReadRepostiory userReadRepo)
		{
			_userReadRepo = userReadRepo;
		}

		public async Task<IReadOnlyList<Guid>> Handle(GetUserManagerIdsQuery request, CancellationToken cancellationToken)
		{
			return await  _userReadRepo.GetManagerIdsAsync(request.UserId, cancellationToken);
		}
	}
}
