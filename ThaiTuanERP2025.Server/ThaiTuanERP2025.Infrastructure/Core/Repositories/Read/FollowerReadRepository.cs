using AutoMapper;
using ThaiTuanERP2025.Application.Core.Followers;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Core.Repositories.Read
{
	public sealed class FollowerReadRepository : BaseReadRepository<Follower, FolloweDto>, IFollowerReadRepository
	{
		public FollowerReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }
	}
}
