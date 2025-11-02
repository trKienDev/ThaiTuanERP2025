using AutoMapper;
using ThaiTuanERP2025.Domain.Followers.Repositories;
using ThaiTuanERP2025.Domain.Followers.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Followers.Repositories
{
	public sealed class FollowerRepository : BaseWriteRepository<Follower>, IFollowerRepository
	{
		public FollowerRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider)
			: base(dbContext, configurationProvider) { }
	}
}
