using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Security;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Commands.Login
{
	public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResultDto>
	{
		private readonly IUserRepository _userRepository;
		private readonly iJWTProvider _jwtProvider;

		public LoginCommandHandler(IUserRepository userRepository, iJWTProvider jwtProvider)
		{
			_userRepository = userRepository;
			_jwtProvider = jwtProvider;
		}

		public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken) { 
			var user = await _userRepository.GetByUsernameAsync(request.Username);
			if (user == null || !PasswordHasher.Verify(request.Password, user.PasswordHash)) {
				throw new AppException("Tên đăng nhập hoặc mật khẩu không đúng");
			}

			if(!user.IsActive) {
				throw new AppException("Tài khoản đã bị khóa!");
			}

			var token = _jwtProvider.GenerateToken(user);

			return new LoginResultDto
			{
				AccessToken = token,
			};
		}
	}
}
