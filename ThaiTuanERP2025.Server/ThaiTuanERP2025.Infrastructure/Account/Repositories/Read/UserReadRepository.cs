using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Account.Departments;
using ThaiTuanERP2025.Application.Account.Roles;
using ThaiTuanERP2025.Application.Account.Users;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories.Read
{
	public class UserReadRepository : BaseReadRepository<User, UserDto>, IUserReadRepostiory
	{
		private ThaiTuanERP2025DbContext DbContext => (ThaiTuanERP2025DbContext)_dbContext;
		public UserReadRepository(ThaiTuanERP2025DbContext context, IMapper mapper)
			: base(context, mapper) { }

		public async Task<IReadOnlyList<UserDto>> ListUserDtosWithAvatarAsync(string? keyword, string? role, Guid? departmentId, CancellationToken cancellationToken = default)
		{
			var query = DbContext.Users
				.AsNoTracking()
				.Include(u => u.Department)
				.Include(u => u.AvatarFile)
				.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
				.Include(u => u.DirectReportsAssignments).ThenInclude(a => a.Manager).ThenInclude(m => m.AvatarFile) 
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(keyword))
			{
				var kw = keyword.Trim();
				query = query.Where(u =>
					EF.Functions.Like(u.FullName, $"%{kw}%") ||
					EF.Functions.Like(u.Username, $"%{kw}%") ||
					EF.Functions.Like(u.EmployeeCode, $"%{kw}%")
				);
			}

			if (!string.IsNullOrWhiteSpace(role))
			{
				var roleName = role.Trim();
				query = query.Where(u => u.UserRoles.Any(r => r.Role.Name == roleName));
			}

			if (departmentId.HasValue)
			{
				query = query.Where(u => u.DepartmentId == departmentId);
			}

			// ==== Project to UserDto ====
			var users = await query.Select(
				u => new UserDto {
					Id = u.Id,
					FullName = u.FullName,
					Username = u.Username,
					EmployeeCode = u.EmployeeCode,
					Position = u.Position,
					Email = u.Email != null ? u.Email.Value : null,
					Phone = u.Phone != null ? u.Phone.Value : null,
					DepartmentId = u.DepartmentId,
					Department = u.Department == null ? null : new DepartmentBriefDto(u.Department.Id, u.Department.Name, u.Department.Code),
					AvatarFileId = u.AvatarFileId.HasValue ? u.AvatarFileId.Value : null,
					AvatarFileObjectKey = u.AvatarFile != null ? u.AvatarFile.ObjectKey : null,

					Roles = u.UserRoles.Select(ur => new RoleDto
					{
						Id = ur.Role.Id,
						Name = ur.Role.Name,
						Description = ur.Role.Description
					}).ToList(),

					Managers = u.DirectReportsAssignments.Where(m => m.RevokedAt == null)
						.Select(m => new UserDto {
							Id = m.Manager.Id,
							FullName = m.Manager.FullName,
							Username = m.Manager.Username,
							Position = m.Manager.Position,
							AvatarFileId = m.Manager.AvatarFileId.HasValue ? m.Manager.AvatarFileId.Value : null,
							AvatarFileObjectKey = m.Manager.AvatarFile != null ? m.Manager.AvatarFile.ObjectKey : null
						}
						).ToList()
				}
			).ToListAsync(cancellationToken);

			return users;
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
