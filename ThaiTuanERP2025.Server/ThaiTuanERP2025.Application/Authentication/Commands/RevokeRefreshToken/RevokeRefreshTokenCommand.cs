using MediatR;

namespace ThaiTuanERP2025.Application.Authentication.Commands.RevokeRefreshToken
{
	public sealed record RevokeRefreshTokenCommand(string RefreshToken, string? IpAddress = null) : IRequest<Unit>;
}
