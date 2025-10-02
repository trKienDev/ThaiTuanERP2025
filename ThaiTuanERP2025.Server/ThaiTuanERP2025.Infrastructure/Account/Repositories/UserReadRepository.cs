using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Domain.Account.Enums;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories
{
	public class UserReadRepository : IUserReadRepository 
	{
		private readonly ThaiTuanERP2025DbContext _dbContext;
		public UserReadRepository(ThaiTuanERP2025DbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<UserDto>> ListUserDtosWithAvatarAsync(string? keyword, string? role, Guid? departmentId, CancellationToken cancellationToken = default)
		{
			var query = _dbContext.Users.AsQueryable();

			if (!string.IsNullOrWhiteSpace(keyword))
				query = query.Where(u => EF.Functions.Like(u.FullName, $"%{keyword}%") || EF.Functions.Like(u.Username, $"%{keyword}%"));

			if (!string.IsNullOrWhiteSpace(role) && Enum.TryParse<UserRole>(role, true, out var parsedRole))
				query = query.Where(u => u.Role == parsedRole);

			return await query.Select(u => new UserDto
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
					? _dbContext.StoredFiles.Where(f => f.Id == u.AvatarFileId.Value).Select(f => f.ObjectKey).FirstOrDefault()
					: null,
				Role = u.Role,
				DepartmentId = u.DepartmentId,
				Department = u.Department == null ? null : new DepartmentDto
				{
					Id = u.Department.Id,
					Name = u.Department.Name,
					Code = u.Department.Code
				}
			}).ToListAsync();
		}
	}
}
