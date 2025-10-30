using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Domain.Account.Repositories
{
	public interface IDepartmentRepository : IBaseRepository<Department>
	{
		Task AddRangeAysnc(IEnumerable<Department> departments);
		Task<bool> ExistAsync(Guid? departmentId);
		Task UpdateAsync(Department department);
		Task DeleteAsync(Guid id);
	}
}
