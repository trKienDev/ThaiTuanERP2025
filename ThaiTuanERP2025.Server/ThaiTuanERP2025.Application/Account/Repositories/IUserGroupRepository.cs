using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Repositories
{
	public interface IUserGroupRepository
	{
		Task<UserGroup?> GetAsync(Guid userId, Guid groupId);	
		Task<List<UserGroup>> GetByGroupIdAsync(Guid groupId);
		Task<List<UserGroup>> GetByUserIdAsync(Guid userId);
		Task AddAsync(UserGroup userGroup, CancellationToken cancellationToken = default);
		Task RemoveAsync(UserGroup userGroup);
		Task<bool> ExistAsync(Guid userId, Guid groupId);
	}
}
