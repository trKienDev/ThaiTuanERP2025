using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Repositories
{
	public interface IUserRepository : IBaseRepository<User>
	{
		Task<User?> GetByUsernameAsync(string username);
		Task<User?> GetByEmployeeCode(string employeeCode);
		Task<List<Guid>> GetManagerIdsAsync(Guid userId, CancellationToken cancellationToken = default);
		Task< List<User>> GetManagersAsync(Guid userId, CancellationToken cancellationToken = default);
	}
}
