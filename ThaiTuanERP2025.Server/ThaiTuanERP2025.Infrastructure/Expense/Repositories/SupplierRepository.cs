using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Repositories;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories
{
	public sealed class SupplierRepository : BaseWriteRepository<Supplier>, ISupplierRepository
	{
		public SupplierRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider) { }

		public Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null, CancellationToken cancellationToken = default)
		{
			var query = _dbSet.AsNoTracking().Where(x => x.Name == name);
			if(excludeId.HasValue) query = query.Where(x => x.Id != excludeId.Value);
			return query.AnyAsync(cancellationToken);
		}

		public async Task<IReadOnlyList<Supplier>> SearchAsync(string? keyword, CancellationToken cancellationToken)
		{
			var query = _dbSet.AsQueryable();
			if (!string.IsNullOrWhiteSpace(keyword))
			{
				var kw = keyword.Trim().ToLower();
				query = query.Where(x => x.Name.ToLower().Contains(kw) || (x.TaxCode != null && x.TaxCode.ToLower().Contains(kw)));
			}
			return await query.OrderBy(x => x.Name).ToListAsync();
		}
	}
}
