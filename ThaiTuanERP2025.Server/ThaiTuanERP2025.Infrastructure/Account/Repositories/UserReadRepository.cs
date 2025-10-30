using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Users;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories
{
	public class UserReadRepository : BaseRepository<User>, IUserReadRepostiory
	{
		private ThaiTuanERP2025DbContext DbContext => (ThaiTuanERP2025DbContext)_context;
		public UserReadRepository(ThaiTuanERP2025DbContext context, IConfigurationProvider configurationProvider)
			: base(context, configurationProvider) { }

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
				query = query.Where(u => u.UserRoles.Any(r => r.Role.Name.Value == role));

			var result = await query
				.Select(u => new UserDto
				{
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
						Name = ur.Role.Name.Value,
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
