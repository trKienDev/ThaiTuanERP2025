using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories
{
	public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
	{
		private readonly ThaiTuanERP2025DbContext _dbContext;
		public DepartmentRepository(ThaiTuanERP2025DbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
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

		public async Task<List<Department>> GetByIdAsync(IEnumerable<Guid> departmentIds, CancellationToken cancellationToken)
		{
			if (departmentIds == null || !departmentIds.Any())
				return new List<Department>();

			var guidList = departmentIds.Select(id => $"'{id}'").ToList();
			var sql = $"SELECT * FROM Departments WHERE Id IN ({string.Join(",", guidList)})";

			return await _dbContext.Departments
			    .FromSqlRaw(sql)
			    .AsNoTracking()
			    .ToListAsync(cancellationToken);
		}
	}
}
