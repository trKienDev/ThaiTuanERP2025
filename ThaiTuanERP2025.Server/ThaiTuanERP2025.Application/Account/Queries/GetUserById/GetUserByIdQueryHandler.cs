using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Repositories;

namespace ThaiTuanERP2025.Application.Account.Queries.GetUserById
{
	public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
	{
		private readonly IUserRepository _userRepository;

		public GetUserByIdQueryHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken) {
			var user = await _userRepository.GetByIdAsync(request.Id);
			if (user == null) throw new Exception("User không tồn tại");

			return new UserDto
			{
				Id = user.Id,
				FullName = user.FullName,
				Username = user.Username,
				Email = user.Email?.Value,
				Phone = user.Phone?.Value,
				Role = user.Role,
				DepartmentId = user.DepartmentId,
			};
		}
	}
}
