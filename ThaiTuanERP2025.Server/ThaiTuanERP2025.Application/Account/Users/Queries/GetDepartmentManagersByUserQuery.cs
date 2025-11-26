using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Account.Departments;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Application.Files;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Account.Users.Repositories;

namespace ThaiTuanERP2025.Application.Account.Users.Queries
{
	public sealed record GetDepartmentManagersByUserQuery() : IRequest<IReadOnlyList<UserBriefAvatarDto>>;
	
	public sealed class GetDepartmentManagersByUserQueryHandler : IRequestHandler<GetDepartmentManagersByUserQuery, IReadOnlyList<UserBriefAvatarDto>> {
		private readonly IUserReadRepostiory _userRepo;
		private readonly ICurrentUserService _currentUser;
		private readonly IDepartmentReadRepository _departmentRepo;
		private readonly IMapper _mapper;
		private readonly IStoredFileReadRepository _fileRepo;
		public GetDepartmentManagersByUserQueryHandler(
			IUserReadRepostiory userRepo, ICurrentUserService currentUser, IDepartmentReadRepository departmentRepo,
			IMapper mapper, IStoredFileReadRepository fileRepo
		) {
			_userRepo = userRepo;
			_currentUser = currentUser;	
			_departmentRepo = departmentRepo;	
			_fileRepo = fileRepo;	
			_mapper = mapper;
		}

		public async Task<IReadOnlyList<UserBriefAvatarDto>> Handle(GetDepartmentManagersByUserQuery query, CancellationToken cancellationToken) {
			var userId = _currentUser.UserId ?? throw new NotFoundException("Không tìm thấy thông tin của user hiện tại");
			var userDto = await _userRepo.GetByIdProjectedAsync(userId, cancellationToken) 
				?? throw new NotFoundException("Không tìm thấy thông tin của user hiện tại");

			if (!userDto.DepartmentId.HasValue)
				throw new NotFoundException("Không tìm thấy phòng ban của user");

			var department = await _departmentRepo.GetWithManagersByIdAsync(userDto.DepartmentId.Value, cancellationToken);
			if (department is null)
				return Array.Empty<UserBriefAvatarDto>();

			var users = department.Managers.Select(m => m.User).ToList();
			var avatarIds = users.Where(u => u.AvatarFileId.HasValue)
				.Select(u => u.AvatarFileId!.Value)
				.Distinct()
				.ToList();

			var avatarDict = avatarIds.Count > 0
				? await _fileRepo.GetObjectKeysByIdsAsync(avatarIds, cancellationToken)
				: new Dictionary<Guid, string>();

			// Map từ entity User → DTO UserBriefAvatarDto
			var result = _mapper.Map<IReadOnlyList<UserBriefAvatarDto>>(users, opt => {
				opt.Items["AvatarDict"] = avatarDict;
			});

			return result;
		}
	}
}
