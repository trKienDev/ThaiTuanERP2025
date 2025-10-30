using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Account.Repositories;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories
{
	public class UserGroupRepository : BaseRepository<UserGroup>, IUserGroupRepository
	{
		private ThaiTuanERP2025DbContext DbContext => (ThaiTuanERP2025DbContext)_context;
		public UserGroupRepository(ThaiTuanERP2025DbContext context, IConfigurationProvider configurationProvider) 
			: base(context, configurationProvider)
		{
		}

		public async Task<UserGroup?> GetAsync(Guid userId, Guid groupId) {
			return await DbContext.UserGroups
				.Include(ug => ug.User)
				.Include(ug => ug.Group)
				.FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GroupId == groupId);
		}

		public async Task<List<UserGroup>> GetByGroupIdAsync(Guid groupId) {
			return await DbContext.UserGroups
				.Where(ug => ug.GroupId == groupId)
				.Include(ug => ug.User)
				.ToListAsync();
		}

		public async Task<List<UserGroup>> GetByUserIdAsync(Guid userId)
		{
			return await DbContext.UserGroups
				.Where(ug => ug.UserId == userId)
				.Include(ug => ug.Group)
				.ToListAsync();
		}

		public new async Task AddAsync(UserGroup userGroup, CancellationToken cancellationToken = default)
		{
			await base.AddAsync(userGroup, cancellationToken);
		}

		public Task RemoveAsync(UserGroup userGroup)
		{
			base.Delete(userGroup);
			return Task.CompletedTask;
		}

		public Task<bool> ExistAsync(Guid userId, Guid groupId)
		{
			return DbContext.UserGroups.AnyAsync(ug => ug.UserId == userId && ug.GroupId == groupId);
		}
	}
}
