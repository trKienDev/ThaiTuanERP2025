using ThaiTuanERP2025.Domain.Authentication.Entities;

namespace ThaiTuanERP2025.Application.Authentication.Repositories
{
	public interface IRefreshTokenRepository
	{
		Task AddAsync(RefreshToken token, CancellationToken ct = default);
		Task<RefreshToken?> GetByTokenAsync(string plainToken, CancellationToken ct = default); // input là token plain, repo tự hash để tìm
		Task SaveChangesAsync(CancellationToken ct = default);
	}
}
