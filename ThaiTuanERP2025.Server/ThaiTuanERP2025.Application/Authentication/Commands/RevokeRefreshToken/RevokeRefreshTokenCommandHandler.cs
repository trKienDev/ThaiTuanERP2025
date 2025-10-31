using MediatR;
using ThaiTuanERP2025.Application.Authentication.Repositories;

namespace ThaiTuanERP2025.Application.Authentication.Commands.RevokeRefreshToken
{
	public sealed class RevokeRefreshTokenCommandHandler : IRequestHandler<RevokeRefreshTokenCommand, Unit>
	{
		private readonly IRefreshTokenRepository _refreshRepo;

		public RevokeRefreshTokenCommandHandler(IRefreshTokenRepository refreshRepo) => _refreshRepo = refreshRepo;

		public async Task<Unit> Handle(RevokeRefreshTokenCommand request, CancellationToken ct)
		{
			var token = await _refreshRepo.GetByTokenAsync(request.RefreshToken, ct)
				?? throw new UnauthorizedAccessException("Refresh token không hợp lệ.");

			token.Revoke(request.IpAddress);
			await _refreshRepo.SaveChangesAsync(ct);
			return Unit.Value;
		}
	}
}
