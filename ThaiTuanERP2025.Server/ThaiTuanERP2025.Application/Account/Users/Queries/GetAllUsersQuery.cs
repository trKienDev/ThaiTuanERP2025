using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;

namespace ThaiTuanERP2025.Application.Account.Users.Queries
{
	public sealed record GetAllUsersQuery(string? keyword, string? role, Guid? departmentId) : IRequest<IReadOnlyList<UserInforDto>>;
	public sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IReadOnlyList<UserInforDto>>
	{
		private readonly IUserReadRepostiory _userReadRepo;
		private readonly IMapper _mapper;
		public GetAllUsersQueryHandler(IUserReadRepostiory userReadRepo, IMapper mapper)
		{
			_userReadRepo = userReadRepo;
			_mapper = mapper;
		}

		public async Task<IReadOnlyList<UserInforDto>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
		{
			return await _userReadRepo.ListProjectedAsync(
			    q => q.Where(u => u.IsActive)
				  .ProjectTo<UserInforDto>(_mapper.ConfigurationProvider),
			    cancellationToken: cancellationToken
			);
		}
	}
}
