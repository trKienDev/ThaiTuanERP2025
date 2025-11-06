using MediatR;
using ThaiTuanERP2025.Application.Authentication.Repositories;

namespace ThaiTuanERP2025.Application.Authentication.Commands
{
	public sealed record RevokeRefreshTokenCommand(string RefreshToken, string? IpAddress = null) : IRequest<Unit>;
	public sealed class RevokeRefreshTokenCommandHandler : IRequestHandler<RevokeRefreshTokenCommand, Unit>
	{
		private readonly IRefreshTokenRepository _refreshRepo;

		public RevokeRefreshTokenCommandHandler(IRefreshTokenRepository refreshRepo) => _refreshRepo = refreshRepo;

		public async Task<Unit> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
		{
			var token = await _refreshRepo.GetByTokenAsync(request.RefreshToken, cancellationToken)
				?? throw new UnauthorizedAccessException("Refresh token không hợp lệ.");

			token.Revoke(request.IpAddress);
			await _refreshRepo.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
