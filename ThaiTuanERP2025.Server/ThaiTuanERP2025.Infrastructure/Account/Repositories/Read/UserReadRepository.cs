using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Application.Account.Users.Repositories;
using ThaiTuanERP2025.Application.Account.Users;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories.Read
{
	public class UserReadRepository : BaseReadRepository<User, UserDto>, IUserReadRepostiory
	{
		private ThaiTuanERP2025DbContext DbContext => (ThaiTuanERP2025DbContext)_dbContext;
		public UserReadRepository(ThaiTuanERP2025DbContext context, IMapper mapper) : base(context, mapper) { }

		public async Task<string?> GetUserNameAsync(Guid userId, CancellationToken cancellationToken)
		{
			return await _dbSet.AsNoTracking()
				.Where(x => x.Id == userId)
				.Select(x => x.FullName)
				.SingleOrDefaultAsync(cancellationToken);
		}

		public Task<UserBriefAvatarDto?> GetBriefWithAvatarAsync(Guid userId, CancellationToken cancellationToken)
		{
			return _dbSet.AsNoTracking()
				.Where(u => u.Id == userId)
				.Select(
					u => new UserBriefAvatarDto {
						Id = u.Id,
						FullName = u.FullName,
						Username = u.Username,
						EmployeeCode = u.EmployeeCode,
						AvatarFileId = u.AvatarFileId
					}
				).SingleOrDefaultAsync(cancellationToken);
		}

		public Task<List<UserBriefAvatarDto>> GetBriefWithAvatarManyAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
		{
			return _dbSet.AsNoTracking()
				.Where(u => userIds.Contains(u.Id))
				.Select(u => new UserBriefAvatarDto
				{
					Id = u.Id,
					FullName = u.FullName,
					Username = u.Username,
					EmployeeCode = u.EmployeeCode,
					AvatarFileId = u.AvatarFileId,
				})
				.ToListAsync(cancellationToken);
		}

		public Task<User?> GetWithRolesAndPermissionsAsync(string employeeCode, CancellationToken cancellationToken)
			=> IncludeRolesAndPermissions().SingleOrDefaultAsync(u => u.EmployeeCode == employeeCode, cancellationToken);

		public Task<User?> GetWithRolesAndPermissionsByIdAsync(Guid userId, CancellationToken cancellationToken)
			=> IncludeRolesAndPermissions().SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);

		private IQueryable<User> IncludeRolesAndPermissions()
			=> _dbSet.Include(u => u.UserRoles)
					.ThenInclude(ur => ur.Role)
						.ThenInclude(r => r.RolePermissions)
							.ThenInclude(rp => rp.Permission);

		public async Task<List<Guid>> GetManagerIdsAsync(Guid userId, CancellationToken cancellationToken = default)
		{
			var ids = await DbContext.UserManagerAssignments.AsNoTracking()
				.Where(x => x.UserId == userId && x.RevokedAt == null)
				.OrderByDescending(x => x.IsPrimary)
				.ThenBy(x => x.AssignedAt)
				.Select(x => x.ManagerId)
				.ToListAsync(cancellationToken);

			if (ids.Count == 0)
			{
				var primary = await DbContext.Users.AsNoTracking()
					.Where(u => u.Id == userId && u.ManagerId != null)
					.Select(u => u.ManagerId!.Value)
					.FirstOrDefaultAsync(cancellationToken);

				if (primary != Guid.Empty) ids.Add(primary);
			}

			return ids;
		}

		public async Task<List<User>> GetManagersAsync(Guid userId, CancellationToken cancellationToken = default)
		{
			var managers = await DbContext.UserManagerAssignments.AsNoTracking()
				.Where(x => x.UserId == userId && x.RevokedAt == null)
				.OrderByDescending(x => x.IsPrimary)
				.ThenBy(x => x.AssignedAt)
				.Select(x => x.Manager)   // navigation tới User (manager)
				.ToListAsync(cancellationToken);

			// (tuỳ chọn) fallback
			if (managers.Count == 0)
			{
				var fallback = await DbContext.Users.AsNoTracking()
					.Where(u => u.Id == userId && u.ManagerId != null)
					.Select(u => u.Manager!)
					.FirstOrDefaultAsync(cancellationToken);

				if (fallback is not null) managers.Add(fallback);
			}

			return managers;
		}
	}
}
