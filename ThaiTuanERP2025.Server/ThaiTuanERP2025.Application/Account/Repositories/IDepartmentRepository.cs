using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Repositories
{
	public interface IDepartmentRepository
	{
		Task AddAsync(Department department, CancellationToken cancellationToken);
		Task AddRangeAysnc(IEnumerable<Department> departments);
		Task<bool> ExistAsync(Guid departmentId);
		Task<List<Department>> GetAllAsync(CancellationToken cancellationToken);
	}
}
