using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories
{
	public class LedgerAccountReadRepository : BaseRepository<LedgerAccount>, ILedgerAccountReadRepository
	{
		private ThaiTuanERP2025DbContext DbContext => (ThaiTuanERP2025DbContext)_context;
		public LedgerAccountReadRepository(ThaiTuanERP2025DbContext context, IConfigurationProvider configurationProvider)
			: base(context, configurationProvider) { }

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
