using MediatR;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using ThaiTuanERP2025.Application.Account.Users;
using ThaiTuanERP2025.Application.Authentication.DTOs;
using ThaiTuanERP2025.Application.Authentication.Repositories;
using ThaiTuanERP2025.Application.Authentication.Services;
using ThaiTuanERP2025.Application.Common.Authentication;
using ThaiTuanERP2025.Application.Common.Security;
using ThaiTuanERP2025.Domain.Authentication.Entities;

namespace ThaiTuanERP2025.Application.Authentication.Commands
{
	public sealed record RefreshAccessTokenCommand(string RefreshToken, string? IpAddress = null) : IRequest<LoginResponseDto>;
	public sealed class RefreshAccessTokenCommandHandler : IRequestHandler<RefreshAccessTokenCommand, LoginResponseDto>
	{
		private readonly IRefreshTokenRepository _refreshRepo;
		private readonly IUserReadRepostiory _userReadRepository;
		private readonly IJWTProvider _jwtProvider;
		private readonly JwtSettings _jwt;

		public RefreshAccessTokenCommandHandler(
			IRefreshTokenRepository refreshRepo, IUserReadRepostiory userReadRepository, IJWTProvider jwtProvider, IOptions<JwtSettings> jwtOptions
		)
		{
			_refreshRepo = refreshRepo;
			_userReadRepository = userReadRepository;
			_jwtProvider = jwtProvider;
			_jwt = jwtOptions.Value;
		}

		public async Task<LoginResponseDto> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
		{
			// 1) Tìm refresh token trong DB
			var tokenEntity = await _refreshRepo.GetByTokenAsync(request.RefreshToken, cancellationToken)
				?? throw new UnauthorizedAccessException("Refresh token không hợp lệ.");

			// 2) Kiểm tra trạng thái
			if (!tokenEntity.IsActive)
				throw new UnauthorizedAccessException("Refresh token đã hết hạn hoặc bị thu hồi.");

			// 3) Lấy user kèm roles/permissions
			var user = await _userReadRepository.GetWithRolesAndPermissionsByIdAsync(tokenEntity.UserId, cancellationToken)
				?? throw new UnauthorizedAccessException("User không tồn tại.");

			// 4) Build claims
			var roles = user.UserRoles.Select(ur => ur.Role.Name).Distinct().ToList();
			var permissions = user.UserRoles
				.SelectMany(ur => ur.Role.RolePermissions.Select(rp => rp.Permission.Code)).Distinct().ToList();

			var claims = new List<Claim>
			{
				new(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new(ClaimTypes.Name, user.Username),
				new(ClaimTypes.Email, user.Email?.Value ?? string.Empty),
			};
			foreach (var r in roles) claims.Add(new Claim(ClaimTypes.Role, r));
			foreach (var p in permissions) claims.Add(new Claim("permission", p));

			// 5) Sinh Access Token mới
			var accessToken = _jwtProvider.GenerateToken(user, claims);
			var accessExpiresAt = DateTime.UtcNow.AddMinutes(_jwt.ExpiryMinutes);

			// 6) Rotation: tạo Refresh Token mới, revoke token cũ
			var newPlainRefresh = RefreshTokenFactory.GenerateSecureToken();
			var newHash = RefreshTokenFactory.ComputeSha256(newPlainRefresh);
			var refreshExpiresAt = DateTime.UtcNow.AddDays(_jwt.RefreshTokenExpiryDays);

			tokenEntity.ReplaceBy(newHash); // revoke + set replaced-by
			var newTokenEntity = new RefreshToken(user.Id, newHash, refreshExpiresAt, request.IpAddress);

			await _refreshRepo.AddAsync(newTokenEntity, cancellationToken);
			await _refreshRepo.SaveChangesAsync(cancellationToken);

			return new LoginResponseDto(
				user.Id,
				user.FullName,
				user.Username,
				accessToken,
				accessExpiresAt,
				roles,
				permissions,
				newPlainRefresh,
				refreshExpiresAt
			);
		}
	}
}
