using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Repositories
{
	public interface IUserReadRepository 
	{
		Task<List<UserDto>> ListUserDtosWithAvatarAsync(string? keyword, string? role, Guid? departmentId, CancellationToken cancellationToken = default);
	}
}
