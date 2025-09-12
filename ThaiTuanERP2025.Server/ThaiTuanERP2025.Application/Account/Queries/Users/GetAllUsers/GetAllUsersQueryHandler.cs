using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Repositories;

namespace ThaiTuanERP2025.Application.Account.Queries.Users.GetAllUsers
{
	public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
	{
		private readonly IUserReadRepository _userReadRepository;
		public GetAllUsersQueryHandler(IUserReadRepository userReadRepository)
		{
			_userReadRepository = userReadRepository;
		}

		public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
		{
			return await _userReadRepository.ListUserDtosWithAvatarAsync(request.Keyword, request.Role, request.DepartmentId, cancellationToken);
		}
	}
}
