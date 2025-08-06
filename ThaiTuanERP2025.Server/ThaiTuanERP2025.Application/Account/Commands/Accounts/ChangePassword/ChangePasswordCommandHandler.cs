using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Application.Common.Security;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Commands.ChangePassword
{
	public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, string>
	{
		private readonly IUserRepository _userRepository;
		public ChangePasswordCommandHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<string> Handle(ChangePasswordCommand request, CancellationToken cancellationToken) {
			var user = await _userRepository.GetByIdAsync(request.UserId) ?? throw new NotFoundException("Người dùng không tồn tại");
			if(!PasswordHasher.Verify(request.CurrentPassword, user.PasswordHash)) {
				throw new AppException("Mật khẩu hiện tại không đúng");
			}

			user.ChangePassword(PasswordHasher.Hash(request.CurrentPassword));
			_userRepository.Update(user);

			return "Đổi mật khẩu thành công";

		}
	}
}
