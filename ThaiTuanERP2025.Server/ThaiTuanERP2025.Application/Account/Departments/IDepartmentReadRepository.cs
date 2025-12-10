using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Departments
{
	public interface IDepartmentReadRepository : IBaseReadRepository<Department, DepartmentDto>
	{
		Task<List<Department>> ListWithManagersAsync(CancellationToken cancellationToken);
		Task<Department?> GetWithManagersByIdAsync(Guid departmentId, CancellationToken cancellationToken);
	}
}
