using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Users.Repositories
{
	public interface IUserReadRepostiory : IBaseReadRepository<User, UserDto>
	{
		Task<string?> GetUserNameAsync(Guid userId, CancellationToken cancellationToken);
		Task<UserBriefAvatarDto?> GetBriefWithAvatarAsync(Guid userId, CancellationToken cancellationToken);
		Task<List<UserBriefAvatarDto>> GetBriefWithAvatarManyAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default);
		Task<User?> GetWithRolesAndPermissionsAsync(string employeeCode, CancellationToken cancellationToken);
		Task<User?> GetWithRolesAndPermissionsByIdAsync(Guid userId, CancellationToken cancellationToken);
		Task<List<Guid>> GetManagerIdsAsync(Guid userId, CancellationToken cancellationToken = default);
	}
}
