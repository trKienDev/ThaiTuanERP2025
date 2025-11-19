using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Users
{
	public interface IUserReadRepostiory : IBaseReadRepository<User, UserDto>
	{
		Task<UserBriefAvatarDto?> GetBriefWithAvatarAsync(Guid userId, CancellationToken cancellationToken);
		Task<User?> GetWithRolesAndPermissionsAsync(string employeeCode, CancellationToken cancellationToken);
		Task<User?> GetWithRolesAndPermissionsByIdAsync(Guid userId, CancellationToken cancellationToken);
		Task<List<Guid>> GetManagerIdsAsync(Guid userId, CancellationToken cancellationToken = default);
	}
}
