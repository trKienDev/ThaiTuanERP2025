using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories
{
	public class UserGroupRepository : IUserGroupRepository
	{
		private readonly ThaiTuanERP2025DbContext _dbContext;
		public UserGroupRepository(ThaiTuanERP2025DbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<UserGroup?> GetAsync(Guid userId, Guid groupId) {
			return await _dbContext.UserGroups
				.Include(ug => ug.User)
				.Include(ug => ug.Group)
				.FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GroupId == groupId);
		}

		public async Task<List<UserGroup>> GetByGroupIdAsync(Guid groupId) {
			return await _dbContext.UserGroups
				.Where(ug => ug.GroupId == groupId)
				.Include(ug => ug.User)
				.ToListAsync();
		}

		public async Task<List<UserGroup>> GetByUserIdAsync(Guid userId)
		{
			return await _dbContext.UserGroups
				.Where(ug => ug.UserId == userId)
				.Include(ug => ug.Group)
				.ToListAsync();
		}

		public async Task AddAsync(UserGroup userGroup)
		{
			await _dbContext.UserGroups.AddAsync(userGroup);
		}

		public Task RemoveAsync(UserGroup userGroup)
		{
			_dbContext.UserGroups.Remove(userGroup);
			return Task.CompletedTask;
		}

		public Task<bool> ExistAsync(Guid userId, Guid groupId)
		{
			return _dbContext.UserGroups.AnyAsync(ug => ug.UserId == userId && ug.GroupId == groupId);
		}
	}
}
