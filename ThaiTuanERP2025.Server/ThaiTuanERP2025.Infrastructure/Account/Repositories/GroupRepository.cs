using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories
{
	public class GroupRepository : IGroupRepository
	{
		private readonly ThaiTuanERP2025DbContext _context;

		public GroupRepository(ThaiTuanERP2025DbContext context)
		{
			_context = context;
		}

		public async Task<Group?> GetByIdAsync(Guid id)
		{
			return await _context.Groups
				.Include(g => g.UserGroups)
				.ThenInclude(ug => ug.User)
				.FirstOrDefaultAsync(g => g.Id == id);
		}

		public async Task<List<Group>> GetAllAsync()
		{
			return await _context.Groups
				.Include(g => g.UserGroups)
				.ThenInclude(ug => ug.User)
				.ToListAsync();
		}

		public async Task AddAsync(Group group)
		{
			await _context.Groups.AddAsync(group);
		}

		public Task DeleteAsync(Group group)
		{
			_context.Groups.Remove(group);
			return Task.CompletedTask;
		}

		public async Task<bool> ExistAsync(Guid id)
		{
			return await _context.Groups.AnyAsync(g => g.Id == id);
		}
	}

}
