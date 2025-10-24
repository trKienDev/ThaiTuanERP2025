using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Repositories
{
	public interface IUserRepository : IBaseRepository<User>
	{
		Task<User?> GetByUsernameAsync(string username);
		Task<User?> GetByEmployeeCode(string employeeCode);
		//Task<Guid?> GetManagerIdAsync(Guid userId, CancellationToken cancellationToken = default);
		Task<List<Guid>> GetManagerIdsAsync(Guid userId, CancellationToken cancellationToken = default);
		Task< List<User>> GetManagersAsync(Guid userId, CancellationToken cancellationToken = default);
		Task<List<UserManagerAssignment>> GetActiveManagerAssignmentsAsync(Guid userId, CancellationToken cancellationToken = default);
		Task AddAssignmentsAsync(IEnumerable<UserManagerAssignment> assignments, CancellationToken cancellationToken = default);
		Task<List<User>> ListByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
		Task<User?> GetWithRolesAndPermissionsAsync(string employeeCode, CancellationToken cancellationToken);
	}
}
