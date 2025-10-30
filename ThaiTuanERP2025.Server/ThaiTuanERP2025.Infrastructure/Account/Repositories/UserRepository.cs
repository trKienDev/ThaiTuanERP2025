using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.RBAC.Dtos;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories
{
	public class UserRepository : BaseRepository<User>, IUserRepository {
		private ThaiTuanERP2025DbContext DbContext => (ThaiTuanERP2025DbContext)_context;
		public UserRepository(ThaiTuanERP2025DbContext context, IConfigurationProvider configurationProvider) 
			: base(context, configurationProvider) {}

		public async Task<List<User>> ListByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
		{
			var idArray = ids?.Distinct().ToArray() ?? Array.Empty<Guid>();
			if (idArray.Length == 0) return new List<User>();

			return await DbContext.Users.Where(u => idArray.Contains(u.Id))
				.AsNoTracking()
				.ToListAsync(cancellationToken);
		}

		public override async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
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

		public async Task<Guid?> GetManagerIdAsync(Guid userId, CancellationToken cancellationToken = default) {
			var managerId = await DbContext.Users.Where(u => u.Id == userId)
				.Select(u => u.ManagerId)
				.FirstOrDefaultAsync(cancellationToken);
			return managerId;
		}

		public async Task<List<Guid>> GetManagerIdsAsync(Guid userId, CancellationToken cancellationToken = default)
		{
			var ids = await DbContext.UserManagerAssignments.AsNoTracking()
				.Where(x => x.UserId == userId && x.RevokedAt == null)
				.OrderByDescending(x => x.IsPrimary)
				.ThenBy(x => x.AssignedAt)
				.Select(x => x.ManagerId)
				.ToListAsync(cancellationToken);

			if (ids.Count == 0) {
				var primary = await DbContext.Users.AsNoTracking()
					.Where(u => u.Id == userId && u.ManagerId != null)
					.Select(u => u.ManagerId!.Value)
					.FirstOrDefaultAsync(cancellationToken);

				if(primary != Guid.Empty) ids.Add(primary);	
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

		public async Task<List<UserManagerAssignment>> GetActiveManagerAssignmentsAsync(Guid userId, CancellationToken cancellationToken = default) {
			return await DbContext.UserManagerAssignments.AsNoTracking()
				      .Where(x => x.UserId == userId && x.RevokedAt == null)
				      .ToListAsync(cancellationToken);
		}

		public Task AddAssignmentsAsync(IEnumerable<UserManagerAssignment> assignments, CancellationToken cancellationToken = default) {
			DbContext.UserManagerAssignments.AddRange(assignments);
			return Task.CompletedTask;
		}

		public async Task<User?> GetWithRolesAndPermissionsAsync(string employeeCode, CancellationToken cancellationToken)
		{
			var result = await _dbSet
			    .Include(u => u.UserRoles)
				.ThenInclude(ur => ur.Role)
				    .ThenInclude(r => r.RolePermissions)
					.ThenInclude(rp => rp.Permission)
			    .SingleOrDefaultAsync(u => u.EmployeeCode == employeeCode, cancellationToken);

			return result;
		}

		public async Task<List<UserDto>> ListUserDtosWithAvatarAsync(string? keyword, string? role, Guid? departmentId, CancellationToken cancellationToken = default)
		{
			var query = DbContext.Users
				.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
				.Include(u => u.DirectReportsAssignments)
					.ThenInclude(a => a.Manager)
				.Include(u => u.Department)
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(keyword))
				query = query.Where(u => EF.Functions.Like(u.FullName, $"%{keyword}%") || EF.Functions.Like(u.Username, $"%{keyword}%"));

			if (departmentId.HasValue)
				query = query.Where(u => u.DepartmentId == departmentId);

			if (!string.IsNullOrWhiteSpace(role))
				query = query.Where(u => u.UserRoles.Any(r => r.Role.Name == role));

			var result = await query
				.Select(u => new UserDto {
				       Id = u.Id,
				       FullName = u.FullName,
				       Username = u.Username,
				       EmployeeCode = u.EmployeeCode,
				       Position = u.Position,
				       Email = u.Email != null ? u.Email.Value : null,
				       Phone = u.Phone != null ? u.Phone.Value : null,
				       AvatarFileId = u.AvatarFileId.HasValue ? u.AvatarFileId.ToString() : null,
				       AvatarFileObjectKey = u.AvatarFileId.HasValue  
						? DbContext.StoredFiles.Where(f => f.Id == u.AvatarFileId.Value)
											.Select(f => f.ObjectKey)
											.FirstOrDefault() : null,
				       DepartmentId = u.DepartmentId,
				       Department = u.Department == null ? null : new DepartmentDto
				       {
					       Id = u.Department.Id,
					       Name = u.Department.Name,
					       Code = u.Department.Code
				       },
				       
				       // 🔹 Lấy danh sách RoleDto
				       Roles = u.UserRoles.Select(ur => new RoleDto
				       {
					       Id = ur.Role.Id,
					       Name = ur.Role.Name,
					       Description = ur.Role.Description
				       }).ToList(),
				       
				       // 🔹 Lấy danh sách ManagerDto (chỉ lấy thông tin cơ bản)
				       Managers = u.DirectReportsAssignments
					       .Where(m => m.RevokedAt == null)
					       .Select(m => new UserDto
					       {
						       Id = m.Manager.Id,
						       FullName = m.Manager.FullName,
						       Username = m.Manager.Username,
						       Position = m.Manager.Position,
						       AvatarFileId = m.Manager.AvatarFileId.ToString(),
						       AvatarFileObjectKey = m.Manager.AvatarFileId.HasValue 
								? DbContext.StoredFiles.Where(f => f.Id == m.Manager.AvatarFileId.Value)
									.Select(f => f.ObjectKey)
									.FirstOrDefault()
						       : null
					       }).ToList()
				}).AsNoTracking().ToListAsync(cancellationToken);

			return result;
		}
	}
}
