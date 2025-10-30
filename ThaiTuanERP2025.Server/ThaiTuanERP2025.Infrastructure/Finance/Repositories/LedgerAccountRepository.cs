using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Finance.Repositories;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories
{
	public class LedgerAccountRepository : BaseRepository<LedgerAccount>, ILedgerAccountRepository
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

		public async Task<List<LedgerAccountLookupDto>> LookupAsync(string? keyword, int take, CancellationToken cancellationToken)
		{
			var query = _dbSet.AsNoTracking();

			if (!string.IsNullOrWhiteSpace(keyword))
			{
				query = query.Where(la =>
					EF.Functions.Like(la.Number, $"%{keyword}%") ||
					EF.Functions.Like(la.Name, $"%{keyword}%")
				 );
			}

			return await query.OrderBy(la => la.Number)
				.Select(la => new LedgerAccountLookupDto(la.Id, la.Number, la.Name, la.Path))
				.Take(take).ToListAsync(cancellationToken);
		}
	}
}
