using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Partner.Repositories;
using ThaiTuanERP2025.Domain.Partner.Entities;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Partner.Repositories
{
	public class SupplierRepository : BaseRepository<Supplier>, ISupplierRepository
	{
		public SupplierRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider) { }

		public Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default) {
			return _dbSet.AnyAsync(x => x.Code == code, cancellationToken);
		}

		public Task<Supplier?> FindByCodeAsync(string code, CancellationToken cancellationToken = default) {
			return _dbSet.FirstOrDefaultAsync(x => x.Code == code, cancellationToken);	
		}

		public Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
		{
			return _dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
		}

		public async Task<(IReadOnlyList<Supplier> Items, int Total)> SearchAsync(string? keyword, bool? isActive, string? currency, int page, int pageSize, CancellationToken cancellationToken = default)
		{
			var query = _dbSet.AsNoTracking();
			if (!string.IsNullOrWhiteSpace(keyword))
			{
				var kw = keyword.Trim().ToUpper();
				query = query.Where(x => 
					x.Name.ToUpper().Contains(kw) || 
					x.Code.ToUpper().Contains(kw) || 
					(x.TaxCode ?? "").ToUpper().Contains(kw)
				);
			}
			if (isActive.HasValue)
			{
				query = query.Where(x => x.IsActive == isActive.Value);
			}
			if (!string.IsNullOrWhiteSpace(currency))
			{
				var ccy = currency.Trim().ToUpper();
				query = query.Where(x => x.DefaultCurrency.ToUpper() == ccy);
			}

			var total = await query.CountAsync(cancellationToken);
			var items = await query.OrderBy(x => x.Code)
				.Skip(Math.Max(0, (page - 1) * pageSize))
				.Take(Math.Clamp(pageSize, 1, 200)).
				ToListAsync(cancellationToken);

			return (items, total);
		}
	}
}
