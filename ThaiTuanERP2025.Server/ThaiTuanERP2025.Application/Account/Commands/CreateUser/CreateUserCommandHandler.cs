using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Common.Security;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Account.Enums;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Application.Account.Commands.CreateUser
{
	public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken) {
			try {
				var email = string.IsNullOrWhiteSpace(request.Email) ? null : new Email(request.Email);
				var phone = string.IsNullOrWhiteSpace(request.Phone) ? null : new Phone(request.Phone);
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

				var departmentExists = await _unitOfWork.Departments.ExistAsync(user.DepartmentId);
				if (!departmentExists)
					throw new Exception("Phòng ban không tồn tại trong DB");

				await _unitOfWork.Users.AddAsync(user);

				var affectedRows = await _unitOfWork.SaveChangesAsync(cancellationToken);
				if(affectedRows == 0)
					throw new Exception("Không thể lưu User vào DB");

				return _mapper.Map<UserDto>(user);
			} catch(Exception ex) {
				Console.WriteLine($"[EXCEPTION] Lỗi khi lưu User: {ex.Message}");
				throw;
			}
		}
	}
}
