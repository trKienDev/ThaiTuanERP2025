using MediatR;
using System.Security.Claims;
using ThaiTuanERP2025.Application.Account.Users.Repositories;
using ThaiTuanERP2025.Application.Files;
using ThaiTuanERP2025.Application.Shared.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Users.Queries
{
	public sealed class GetProfilleQuery : IRequest<UserDto>
	{
		public ClaimsPrincipal UserPrincipal { get; }
		public GetProfilleQuery(ClaimsPrincipal userPrincipal)
		{
			UserPrincipal = userPrincipal;
		}
	}

	public sealed class GetProfilleQueryHandler : IRequestHandler<GetProfilleQuery, UserDto>
	{
		private readonly IUserReadRepostiory _userReadRepostiory;
		private readonly IStoredFileReadRepository _fileRepo;
		public GetProfilleQueryHandler(IUserReadRepostiory userReadRepostiory, IStoredFileReadRepository fileRepo)
		{
			_userReadRepostiory = userReadRepostiory;
			_fileRepo = fileRepo;
		}

		public async Task<UserDto> Handle(GetProfilleQuery query, CancellationToken cancellationToken)
		{
			var userIdClaim = query.UserPrincipal.FindFirst(ClaimTypes.NameIdentifier)
				?? throw new NotFoundException("Không tìm thấy ID người dùng");

			if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
				throw new ValidationException("Id người dùng không hợp lệ");

			var userDto = await _userReadRepostiory.GetByIdProjectedAsync(userId, cancellationToken)
				?? throw new NotFoundException("Không tìm thấy người dùng");

			return userDto;
		}
	}
}
