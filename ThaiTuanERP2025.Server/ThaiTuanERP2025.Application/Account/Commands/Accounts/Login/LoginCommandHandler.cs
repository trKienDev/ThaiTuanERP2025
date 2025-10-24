using MediatR;
using System.Security.Claims;
using ThaiTuanERP2025.Application.Authentication;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Security;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Commands.Login
{
	public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResultDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly iJWTProvider _jwtProvider;

		public LoginCommandHandler(IUnitOfWork unitOfWork, iJWTProvider jwtProvider)
		{
			_unitOfWork = unitOfWork;
			_jwtProvider = jwtProvider;
		}

		public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken) {
			var user = await _unitOfWork.Users.GetWithRolesAndPermissionsAsync(request.EmployeeCode, cancellationToken);

			if (user == null || !PasswordHasher.Verify(request.Password, user.PasswordHash))
				throw new AppException("Tên đăng nhập hoặc mật khẩu không đúng");

			if (!user.IsActive)
				throw new AppException("Tài khoản đã bị khóa!");

			// Lấy danh sách Role và Permission
			var roles = user.UserRoles.Select(ur => ur.Role.Name).Distinct().ToList();
			var permissions = user.UserRoles.SelectMany(ur => ur.Role.RolePermissions.Select(rp => rp.Permission.Code)).Distinct().ToList();

			// Tạo Claims
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.Username),
				new Claim("fullName", user.FullName)
			};

			// Thêm Role
			claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

			// Thêm Permission
			claims.AddRange(permissions.Select(p => new Claim("permissions", p)));

			// Sinh JWT token
			var token = _jwtProvider.GenerateToken(user, claims);

			// Trả về DTO
			return new LoginResultDto
			{
				AccessToken = token,
				FullName = user.FullName,
				Username = user.Username,
				Roles = roles,
				Permissions = permissions
			};
		}
	}
}
