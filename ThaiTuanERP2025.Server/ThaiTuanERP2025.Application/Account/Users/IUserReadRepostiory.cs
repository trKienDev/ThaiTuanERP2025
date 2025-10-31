using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Repositories;

namespace ThaiTuanERP2025.Application.Account.Users
{
	public interface IUserReadRepostiory : IBaseRepository<User>
	{
		Task<List<UserDto>> ListUserDtosWithAvatarAsync(string? keyword, string? role, Guid? departmentId, CancellationToken cancellationToken = default);
		Task<User?> GetWithRolesAndPermissionsAsync(string employeeCode, CancellationToken cancellationToken);
		Task<User?> GetWithRolesAndPermissionsByIdAsync(Guid userId, CancellationToken cancellationToken);
	}
}
