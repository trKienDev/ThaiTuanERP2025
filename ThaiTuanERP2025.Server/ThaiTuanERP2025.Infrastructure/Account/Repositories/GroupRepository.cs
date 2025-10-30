using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Account.Repositories;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories
{
	public class GroupRepository : BaseRepository<Group>, IGroupRepository
	{
		private ThaiTuanERP2025DbContext DbContext => (ThaiTuanERP2025DbContext)_context;
		public GroupRepository(ThaiTuanERP2025DbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
		{
		}

		public async Task<bool> ExistAsync(Guid id)
		{
			return await DbContext.Groups.AnyAsync(g => g.Id == id);
		}

		public override async Task<Group?> GetByIdAsync(Guid id, CancellationToken cancellationToken) { 
			return await DbContext.Groups
				.Include(g => g.UserGroups)
				.ThenInclude(ug => ug.User)
				.FirstOrDefaultAsync(g => g.Id == id);
		}

		public override async Task<List<Group>> GetAllAsync(CancellationToken cancellationToken) {
			return await DbContext.Groups
				.Include(g => g.UserGroups)
				.ThenInclude(ug => ug.User)
				.ToListAsync();
		}
	}

}
