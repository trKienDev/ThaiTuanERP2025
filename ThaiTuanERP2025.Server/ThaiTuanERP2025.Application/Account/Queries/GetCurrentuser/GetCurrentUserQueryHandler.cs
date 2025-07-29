using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Queries.GetCurrentuser
{
	public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentuserQuery, UserDto> 
	{
		private readonly IUserRepository _userRepository;
		public GetCurrentUserQueryHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<UserDto> Handle(GetCurrentuserQuery request, CancellationToken cancellationToken) {
			var userIdClaim = request.UserPrincipal.FindFirst(ClaimTypes.NameIdentifier)
				?? throw new AppException("Không tìm thấy ID người dùng");
			if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
				throw new AppException("Id người dùng không hợp lệ");
			var user = await _userRepository.GetByIdAsync(userId)
				?? throw new NotFoundException("Người dùng không tồn tại");

			return new UserDto
			{
				Id = user.Id,
				Username = user.Username,
				FullName = user.FullName,
				Email = user.Email?.Value,
				Phone = user.Phone?.Value,
				Role = user.Role,
				DepartmentId = user.DepartmentId,
			};
		}
	}
}
