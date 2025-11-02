using AutoMapper;
using MediatR;
using System.Security.Claims;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Exceptions;
using ThaiTuanERP2025.Application.Files;

namespace ThaiTuanERP2025.Application.Account.Users.Queries.Profile
{
	public sealed class GetProfilleQueryHandler : IRequestHandler<GetProfilleQuery, UserDto>
	{
		private readonly IUserReadRepostiory _userReadRepostiory;
		private readonly IMapper _mapper;
		private readonly IStoredFileReadRepository _storedFileReadRepository;
		public GetProfilleQueryHandler(
			IMapper mapper, IUserReadRepostiory userReadRepostiory, IStoredFileReadRepository storedFileReadRepository
		)
		{
			_userReadRepostiory = userReadRepostiory;
			_mapper = mapper;
			_storedFileReadRepository = storedFileReadRepository;
		}

		public async Task<UserDto> Handle(GetProfilleQuery query, CancellationToken cancellationToken) {
			var userIdClaim = query.UserPrincipal.FindFirst(ClaimTypes.NameIdentifier)
				?? throw new NotFoundException("Không tìm thấy ID người dùng");

			if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
				throw new ValidationException("Id người dùng không hợp lệ");

			var user = await _userReadRepostiory.GetByIdAsync(userId, cancellationToken)
				?? throw new NotFoundException("Không tìm thấy người dùng");

			var dto = _mapper.Map<UserDto>(user);
			if (user.AvatarFileId.HasValue)
			{
				var storedFile = await _storedFileReadRepository.GetByIdAsync(user.AvatarFileId.Value);
				if (storedFile != null)
				{
					dto.AvatarFileObjectKey = storedFile.ObjectKey;
				}
			}
			return dto;
		}
	}
}
