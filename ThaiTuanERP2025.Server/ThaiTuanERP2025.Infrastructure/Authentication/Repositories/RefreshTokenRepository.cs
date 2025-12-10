using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Authentication.Repositories;
using ThaiTuanERP2025.Application.Authentication.Services;
using ThaiTuanERP2025.Domain.Authentication.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Authentication.Repositories
{
	public sealed class RefreshTokenRepository : IRefreshTokenRepository
	{
		private readonly ThaiTuanERP2025DbContext _db;

		public RefreshTokenRepository(ThaiTuanERP2025DbContext db) => _db = db;

		public Task AddAsync(RefreshToken token, CancellationToken ct = default)
			=> _db.RefreshTokens.AddAsync(token, ct).AsTask();

		public async Task<RefreshToken?> GetByTokenAsync(string plainToken, CancellationToken ct = default)
		{
			var hash = RefreshTokenFactory.ComputeSha256(plainToken);
			return await _db.RefreshTokens.AsTracking().SingleOrDefaultAsync(x => x.TokenHash == hash, ct);
		}

		public Task SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
	}
}
