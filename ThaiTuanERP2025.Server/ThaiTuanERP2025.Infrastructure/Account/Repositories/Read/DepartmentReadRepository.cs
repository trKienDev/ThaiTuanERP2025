using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Account.Departments;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories.Read
{
	public class DepartmentReadRepository : BaseReadRepository<Department, DepartmentDto>, IDepartmentReadRepository
	{
		public DepartmentReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapper)  : base(dbContext, mapper) { }

		public async Task<List<Department>> ListWithManagersAsync(CancellationToken cancellationToken)
		{
			return await _dbSet.AsNoTracking()
				.Where(d => d.IsActive)
				.Include(d => d.Managers).ThenInclude(m => m.User)
				.OrderByDescending(d => d.Level)
				.ToListAsync(cancellationToken);
		}

		public async Task<Department?> GetWithManagersByIdAsync(Guid departmentId, CancellationToken cancellationToken)
		{
			return await _dbSet.AsNoTracking()
				.Where(d => d.Id == departmentId && d.IsActive)
				.Include(d => d.Managers)
					.ThenInclude(m => m.User)
				.FirstOrDefaultAsync(cancellationToken);
		}
	}


}
