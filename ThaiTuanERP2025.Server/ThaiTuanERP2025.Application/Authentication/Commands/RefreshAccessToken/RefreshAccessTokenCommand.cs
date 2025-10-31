using MediatR;
using ThaiTuanERP2025.Application.Authentication.DTOs;

namespace ThaiTuanERP2025.Application.Authentication.Commands.RefreshAccessToken
{
	public sealed record RefreshAccessTokenCommand(string RefreshToken, string? IpAddress = null) : IRequest<LoginResponseDto>;
}
