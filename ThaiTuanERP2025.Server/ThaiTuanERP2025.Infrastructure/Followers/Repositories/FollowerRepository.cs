using AutoMapper;
using ThaiTuanERP2025.Application.Followers.Repositories;
using ThaiTuanERP2025.Domain.Followers.Entities;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Followers.Repositories
{
	public sealed class FollowerRepository : BaseRepository<Follower>, IFollowerRepository
	{
		public FollowerRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider)
			: base(dbContext, configurationProvider) { }
	}
}
