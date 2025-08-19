using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Finance.Repositories;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories
{
	public class LedgerAccountRepository : BaseRepository<LedgerAccount>, ILedgerAccountRepository
	{
		public LedgerAccountRepository(ThaiTuanERP2025DbContext dbContext) : base(dbContext) { }

		public Task<bool> NumberExistsAsync(string number, Guid? excludeId = null, CancellationToken cancellationToken = default)
		{
			var query = _dbSet.AsNoTracking().Where(x => x.Number == number);
			if(excludeId.HasValue)
			{
				query = query.Where(x => x.Id != excludeId.Value);
			}
			return query.AnyAsync(cancellationToken);
		}

		public Task<List<LedgerAccount>> GetSubtreeAsync(string pathPrefix, bool asNoTracking = true,CancellationToken cancellationToken = default)
		{
			IQueryable<LedgerAccount> query = _dbSet.Where(x => x.Path.StartsWith(pathPrefix));
			if (asNoTracking) query = query.AsNoTracking();
			return query.OrderBy(x => x.Path).ToListAsync(cancellationToken);
		}
	}
}
