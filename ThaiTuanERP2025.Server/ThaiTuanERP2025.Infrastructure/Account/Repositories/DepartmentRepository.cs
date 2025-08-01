using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories
{
	public class DepartmentRepository : IDepartmentRepository
	{
		private readonly ThaiTuanERP2025DbContext _dbContext;
		public DepartmentRepository(ThaiTuanERP2025DbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		public async Task<List<Department>> GetAllAsync(CancellationToken cancellationToken) {
			return await _dbContext.Departments.ToListAsync(cancellationToken);
		}

		public async Task AddRangeAysnc(IEnumerable<Department> departments)
		{
			if (departments == null || !departments.Any()) throw new ArgumentNullException(nameof(departments));
			await _dbContext.Departments.AddRangeAsync(departments);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<bool> ExistAsync(Guid departmentId)
		{
			return await _dbContext.Departments.AnyAsync(d => d.Id == departmentId);
		}

		public async Task AddAsync(Department department, CancellationToken cancellationToken)
		{
			if (department == null) throw new ArgumentNullException(nameof(department));
			await _dbContext.Departments.AddAsync(department, cancellationToken);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
