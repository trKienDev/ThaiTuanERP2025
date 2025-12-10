using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Core.OutboxMessages;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Core.Repositories.Read
{
	public sealed class OutboxMessageReadRepository : BaseReadRepository<OutboxMessage, OutboxMessageDto>, IOutboxMessageReadRepository
	{
		public OutboxMessageReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

		public async Task<List<OutboxMessage>> GetUnprocessedAsync(int batchSize, CancellationToken cancellationToken = default)
		{
			return await _dbSet.Where(x => x.ProcessedOnUtc == null && x.RetryCount < 5)
				.OrderBy(x => x.OccurredOnUtc)
				.Take(batchSize)
				.ToListAsync(cancellationToken);
		}
	}
}
