using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Domain.Core.Repositories
{
	public interface IOutboxMessageWriteRepository : IBaseWriteRepository<OutboxMessage>
	{
	}
}
