using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Application.Common.Security;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Account.Enums;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Application.Account.Commands.CreateUser
{
	public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
	{
		private readonly IUserRepository _userRepository;
		public CreateUserCommandHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken) {
			var email = string.IsNullOrWhiteSpace(request.Email) ? null : new Email(request.Email);
			var phone = string.IsNullOrWhiteSpace(request.Phone)  ? null : new Phone(request.Phone);
			var role = Enum.Parse<UserRole>(request.Role, true);

			var user = new User(
				fullName: request.FullName,
				userName: request.Username,
				employeeCode: request.EmployeeCode,
				passwordHash: PasswordHasher.Hash(request.Password),
				avatarUrl: request.AvatarUrl,
				role: role,
				position: request.Position,
				departmentId: request.DepartmentId,
				email: email,
				phone: phone
			);

			await _userRepository.AddAsync( user );
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
