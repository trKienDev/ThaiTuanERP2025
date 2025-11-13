using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Finance.Repositories;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories
{
	public class LedgerAccountRepository : BaseWriteRepository<LedgerAccount>, ILedgerAccountRepository
	{
		public LedgerAccountRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider) { }

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
