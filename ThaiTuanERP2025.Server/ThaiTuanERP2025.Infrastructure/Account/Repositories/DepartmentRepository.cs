using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Account.Repositories;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories
{
	public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
	{
		private readonly ThaiTuanERP2025DbContext _dbContext;
		public DepartmentRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		public async Task AddRangeAysnc(IEnumerable<Department> departments)
		{
			if (departments == null || !departments.Any()) throw new ArgumentNullException(nameof(departments));
			await _dbContext.Departments.AddRangeAsync(departments);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<bool> ExistAsync(Guid? departmentId)
		{
			return await _dbContext.Departments.AnyAsync(d => d.Id == departmentId);
		}

		public async Task UpdateAsync(Department department)
		{
			_dbContext.Departments.Update(department);
			await _dbContext.SaveChangesAsync();
		}

		public async Task DeleteAsync(Guid id)
		{
			var entity = await _dbContext.Departments.FindAsync(id);
			if (entity != null)
			{
			_dbContext.Departments.Remove(entity);
			await _dbContext.SaveChangesAsync();
			}
		}
	}
}
