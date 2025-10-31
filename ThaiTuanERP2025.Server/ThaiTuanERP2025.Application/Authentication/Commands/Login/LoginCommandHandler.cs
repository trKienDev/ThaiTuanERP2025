using MediatR;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using ThaiTuanERP2025.Application.Account.Users;
using ThaiTuanERP2025.Application.Authentication.DTOs;
using ThaiTuanERP2025.Application.Authentication.Repositories;
using ThaiTuanERP2025.Application.Authentication.Services;
using ThaiTuanERP2025.Application.Common.Authentication;
using ThaiTuanERP2025.Application.Common.Security;
using ThaiTuanERP2025.Application.Common.Services;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Authentication.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Application.Authentication.Commands.Login
{
	public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
	{
		private readonly IUserReadRepostiory _userReadRepostiory;
		private readonly IJWTProvider _jwtProvider;
		private readonly IPasswordHasher _passwordHasher;
		private readonly ILoggingService _logger;
		private readonly JwtSettings _jwtSettings;
		private readonly IRefreshTokenRepository _refreshTokenRepository;
		private readonly ICurrentRequestIpProvider? _currentRequestIpProvider;

		public LoginCommandHandler(
			IUserReadRepostiory userReadRepostiory, IJWTProvider jwtProvider, IPasswordHasher passwordHasher, 
			ILoggingService logger, IOptions<JwtSettings> jwtOptions, IRefreshTokenRepository refreshTokenRepository,
			ICurrentRequestIpProvider? currentRequestIpProvider = null
		) {
			_userReadRepostiory = userReadRepostiory;
			_jwtProvider = jwtProvider;
			_passwordHasher = passwordHasher;
			_logger = logger;
			_jwtSettings = jwtOptions.Value;
			_refreshTokenRepository = refreshTokenRepository;
			_currentRequestIpProvider = currentRequestIpProvider;
		}

		public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
		{
			// 1 ) Guard
			Guard.AgainstNull(request, nameof(request));
			Guard.AgainstNullOrWhiteSpace(request.EmployeeCode, nameof(request.EmployeeCode));
			Guard.AgainstNullOrWhiteSpace(request.Password, nameof(request.Password));

			var user = await _userReadRepostiory.GetWithRolesAndPermissionsAsync(request.EmployeeCode, cancellationToken);
			if (user is null) 
				throw new UnauthorizedAccessException("Tài khoản không hợp lệ");

			if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
				throw new UnauthorizedAccessException("Sai mật khẩu");

			// 2 ) Claims
			var (roles, permissions, claims) = BuildClaims(user);

			// 3 ) Access token
			var accessToken = _jwtProvider.GenerateToken(user, claims);
			var accessExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

			// 4 ) Refresh token (rotation seed)
			var plainRefresh = RefreshTokenFactory.GenerateSecureToken();
			var refreshHash = RefreshTokenFactory.ComputeSha256(plainRefresh);
			var refreshExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays);

			var ip = _currentRequestIpProvider?.GetIp();
			var refreshEntity = new RefreshToken(user.Id, refreshHash, refreshExpiresAt, ip);

			await _refreshTokenRepository.AddAsync(refreshEntity, cancellationToken);
			await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

			_logger.LogInformation($"User {user.EmployeeCode} logged in. Issued Access Token and Refresh Token.", user.EmployeeCode);

			// return
			return new LoginResponseDto(
				user.Id,
				user.FullName,
				user.Username,
				accessToken,
				accessExpiresAt,
				roles,
				permissions,
				plainRefresh,
				refreshExpiresAt
			);
		}

		private (List<string> Roles, List<string> Permissions, List<Claim> Claims) BuildClaims(User user)
		{
			var roles = user.UserRoles
				.Select(ur => ur.Role.Name.Value)
				.Distinct()
				.ToList();

			var permissions = user.UserRoles
				.SelectMany(ur => ur.Role.RolePermissions.Select(rp => rp.Permission.Code))
				.Distinct()
				.ToList();

			var claims = new List<Claim>
			{
				new(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new(ClaimTypes.Name, user.Username),
				new(ClaimTypes.Email, user.Email?.Value ?? string.Empty)
			};

			foreach (var role in roles)
				claims.Add(new Claim(ClaimTypes.Role, role));

			foreach (var permission in permissions)
				claims.Add(new Claim("permission", permission));

			return (roles, permissions, claims);
		}
	}
}
