using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Expense.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories
{
	public sealed class SupplierRepository : BaseRepository<Supplier>, ISupplierRepository
	{
		public SupplierRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider) { }

		public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
		{
			return await _dbSet.AnyAsync(x => x.Name.ToLower() == name.ToLower(), cancellationToken);
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
