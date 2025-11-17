using AutoMapper;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Core.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Core.Repositories.Write
{
	public sealed class OutboxMessageWriteRepository : BaseWriteRepository<OutboxMessage>, IOutboxMessageWriteRepository
	{
		public OutboxMessageWriteRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider mapperConfig)
			: base(dbContext, mapperConfig) { }
	}
}
