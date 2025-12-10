using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ThaiTuanERP2025.Application.Account.Users.Repositories;

namespace ThaiTuanERP2025.Application.Account.Users.Queries
{
	public sealed record SearchUserByNameQuery(string Keyword) : IRequest<IReadOnlyList<UserBriefAvatarDto>>;
	public sealed class SearchUserByNameQueryHandler : IRequestHandler<SearchUserByNameQuery, IReadOnlyList<UserBriefAvatarDto>>
	{
		private readonly IUserReadRepostiory _userRepo;
		private readonly IMapper _mapper;
		public SearchUserByNameQueryHandler(IUserReadRepostiory userRepo, IMapper mapper)
		{
			_userRepo = userRepo;	
			_mapper = mapper;	
		}

		public async Task<IReadOnlyList<UserBriefAvatarDto>> Handle(SearchUserByNameQuery query, CancellationToken cancellationToken)
		{
			var keyword = query.Keyword.Trim();

			if (string.IsNullOrWhiteSpace(keyword) || keyword.Length < 1)
				return new List<UserBriefAvatarDto>();

			keyword = keyword.ToLower();

			return await _userRepo.ListProjectedAsync(
				q => q.Where(
					x => x.IsActive &&
					(
						x.FullName.ToLower().Contains(keyword) ||
						x.Username.ToLower().Contains(keyword) ||
						x.EmployeeCode.ToLower().Contains(keyword)
					)
				).ProjectTo<UserBriefAvatarDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			);
		}
	}
}
