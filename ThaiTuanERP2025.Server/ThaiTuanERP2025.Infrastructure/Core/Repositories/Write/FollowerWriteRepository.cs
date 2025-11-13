using AutoMapper;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Core.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Core.Repositories
{
	public sealed class FollowerWriteRepository : BaseWriteRepository<Follower>, IFollowerWriteRepository
	{
		public FollowerWriteRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider)
			: base(dbContext, configurationProvider) { }
	}
}
