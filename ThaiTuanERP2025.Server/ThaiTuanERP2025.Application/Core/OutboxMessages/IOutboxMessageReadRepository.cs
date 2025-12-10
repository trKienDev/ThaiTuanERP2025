using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Application.Core.OutboxMessages
{
	public interface IOutboxMessageReadRepository : IBaseReadRepository<OutboxMessage, OutboxMessageDto>
	{
		Task<List<OutboxMessage>> GetUnprocessedAsync(int batchSize, CancellationToken cancellationToken = default);
	}
}
