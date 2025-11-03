using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Repositories;

namespace ThaiTuanERP2025.Domain.Account.Repositories
{
	public interface IUserWriteRepository : IBaseWriteRepository<User>
	{
		Task AddAssignmentsAsync(IEnumerable<UserManagerAssignment> assignments, CancellationToken cancellationToken = default);
		Task<List<UserManagerAssignment>> GetActiveManagerAssignmentsAsync(Guid userId, CancellationToken cancellationToken = default);
	}
}
