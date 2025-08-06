using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Domain.Account.Enums;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Commands.Users.UpdateUser
{
	public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
	{
		private readonly IUserRepository _userRepository;
		public UpdateUserCommandHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException("Người dùng không tồn tại");
			var email = string.IsNullOrWhiteSpace(request.Email) ? null : new Email(request.Email);
			var phone = string.IsNullOrWhiteSpace(request.Phone) ? null : new Phone(request.Phone);
			var role = Enum.Parse<UserRole>(request.Role, true);

			user.UpdateProfile(
				fullName: request.FullName,
				avatarUrl: request.AvatarUrl,
				position: request.Position,
				email: email,
				phone: phone
			);

			// Update các trường liên quan
			user.SetRole(role);
			user.SetDepartment(request.DepartmentId);

			if (request.IsActive) user.Activate();
			else user.Deactivate();

			_userRepository.Update(user);

			return new UserDto
			{
				Id = user.Id,
				FullName = request.FullName,
				Username = user.Username,
				Email = user.Email?.Value,
				Phone = user.Phone?.Value,
				Role = user.Role,
				DepartmentId = user.DepartmentId
			};
		}
	}
}
