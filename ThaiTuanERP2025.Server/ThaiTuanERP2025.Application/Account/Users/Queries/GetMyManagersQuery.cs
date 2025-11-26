using MediatR;
using ThaiTuanERP2025.Application.Account.Users.Repositories;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;

namespace ThaiTuanERP2025.Application.Account.Users.Queries
{
	public sealed record GetMyManagersQuery() : IRequest<IReadOnlyList<UserBriefAvatarDto>>;

	public sealed class GetMyManagersQueryHandler : IRequestHandler<GetMyManagersQuery, IReadOnlyList<UserBriefAvatarDto>>
	{
		private readonly IUserReadRepostiory _userRepo;
		private readonly IUserManagerAssignmentReadRepository _managerAssignRepo;
		private readonly ICurrentUserService _currentUser;
		public GetMyManagersQueryHandler(
			IUserReadRepostiory userRepo, IUserManagerAssignmentReadRepository managerAssignRepo, ICurrentUserService currentUser	
		) {
			_userRepo = userRepo;
			_managerAssignRepo = managerAssignRepo;
			_currentUser = currentUser;
		}

		public async Task<IReadOnlyList<UserBriefAvatarDto>> Handle(GetMyManagersQuery query, CancellationToken cancellationToken)
		{
			var userId = _currentUser.UserId ?? throw new NotFoundException("Id của user không hợp lệ");
			var userExist = await _userRepo.ExistAsync(q => q.Id == userId, cancellationToken);
			if (!userExist) throw new NotFoundException("User của bạn không hợp lệ");

			var managerIds = await _managerAssignRepo.GetActiveManagerIdsAsync(userId, cancellationToken);
			if(managerIds.Count  == 0)
				return Array.Empty<UserBriefAvatarDto>();

			return await _userRepo.GetBriefWithAvatarAsync(managerIds, cancellationToken);
		}
	}
}
