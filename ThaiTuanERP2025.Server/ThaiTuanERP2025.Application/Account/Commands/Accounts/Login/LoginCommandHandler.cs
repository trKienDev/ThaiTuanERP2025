using MediatR;
using System.Security.Claims;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Common.Authentication;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Security;

namespace ThaiTuanERP2025.Application.Account.Commands.Login
{
	public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IJWTProvider _jwtProvider;
		private readonly IPasswordHasher _passwordHasher;

		public LoginCommandHandler(IUnitOfWork unitOfWork, IJWTProvider jwtProvider, IPasswordHasher passwordHasher)
		{
			_unitOfWork = unitOfWork;
			_jwtProvider = jwtProvider;
			_passwordHasher = passwordHasher;
		}

		public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken) {
			var user = await _unitOfWork.Users.GetWithRolesAndPermissionsAsync(request.EmployeeCode, cancellationToken);
			if(user is null) throw new UnauthorizedAccessException("Invalid credentials.");

			if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
				throw new UnauthorizedAccessException("Invalid credentials.");

			// Claims
			var roles = user.UserRoles.Select(ur => ur.Role.Name).Distinct().ToList();
			var permissions = user.UserRoles.SelectMany(ur => ur.Role.RolePermissions.Select(rp => rp.Permission.Code)).Distinct().ToList();

			var claims = new List<Claim>
			{
				new(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new(ClaimTypes.Name, user.Username),
				new(System.Security.Claims.ClaimTypes.Email, user.Email?.Value ?? string.Empty)
			};

			roles.ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));
			permissions.ForEach(p => claims.Add(new Claim("permission", p)));

			var token = _jwtProvider.GenerateToken(user, claims);
			var expiresAt = DateTime.UtcNow.AddMinutes(1440); // hoặc lấy từ cấu hình JwtSettings

			return new LoginResponseDto(user.Id, user.FullName, user.Username, token, expiresAt, roles, permissions);
		}
	}
}
