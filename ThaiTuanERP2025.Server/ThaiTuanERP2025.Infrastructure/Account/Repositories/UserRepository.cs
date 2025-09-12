using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Account.Enums;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories
{
	public class UserRepository : BaseRepository<User>, IUserRepository {
		private ThaiTuanERP2025DbContext DbContext => (ThaiTuanERP2025DbContext)_context;
		public UserRepository(ThaiTuanERP2025DbContext context, IConfigurationProvider configurationProvider) 
			: base(context, configurationProvider)
		{

		}

		public override async Task<User?> GetByIdAsync(Guid id) {
			return await DbContext.Users
				.Include(u => u.Department)
				.Include(u => u.UserGroups).ThenInclude(ug => ug.Group)
				.FirstOrDefaultAsync(u => u.Id == id);
		}

		public override async Task<List<User>> GetAllAsync()
		{
			return await DbContext.Users
				.Include(u => u.Department)
				.Include(u => u.UserGroups).ThenInclude(ug => ug.Group)
				.ToListAsync();
		}

		public async Task<User?> GetByUsernameAsync(string username)
		{
			return await DbContext.Users
				.Include(u => u.UserGroups).ThenInclude(ug => ug.Group)
				.FirstOrDefaultAsync(u => u.Username == username);
		}

		public async Task<User?> GetByEmployeeCode(string employeeCode) {
			return await DbContext.Users
			      .Include(u => u.UserGroups).ThenInclude(ug => ug.Group)
			      .FirstOrDefaultAsync(u => u.EmployeeCode == employeeCode);
		}
	}
}
