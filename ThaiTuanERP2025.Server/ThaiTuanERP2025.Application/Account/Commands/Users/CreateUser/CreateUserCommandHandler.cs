using AutoMapper;
using MediatR;
using System.Runtime.Intrinsics.X86;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Common.Security;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Account.Enums;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Application.Account.Commands.Users.CreateUser
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

		public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
		{
			try
			{
				var email = string.IsNullOrWhiteSpace(request.Email) ? null : new Email(request.Email);
				var phone = string.IsNullOrWhiteSpace(request.Phone) ? null : new Phone(request.Phone);
				var role = Enum.Parse<UserRole>(request.Role, true);

				var user = new User(
					fullName: request.FullName,
					userName: request.Username,
					employeeCode: request.EmployeeCode,
					passwordHash: PasswordHasher.Hash(request.Password),
					role: role,
					position: request.Position,
					departmentId: request.DepartmentId,
					email: email,
					phone: phone
				);

				var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId);
				if (department is null)
					throw new Exception("Phòng ban không tồn tại trong DB");
				if (department.ManagerUserId is Guid mgrId)
				{
					var manager = await _unitOfWork.Users.GetByIdAsync(mgrId);
					if (manager is not null)
					{
						// assignment nguồn chính
						var assignment = new UserManagerAssignment
						{
							UserId = user.Id,
							User = user,
							ManagerId = manager.Id,
							IsPrimary = true,
							AssignedAt = DateTime.UtcNow
						};
						user.DirectReportsAssignments.Add(assignment); // :contentReference[oaicite:4]{index=4}:contentReference[oaicite:5]{index=5}
						user.AssignManager(mgrId); // chặn self-management trong domain :contentReference[oaicite:6]{index=6}
					}
				}

				await _unitOfWork.Users.AddAsync(user);

				var affectedRows = await _unitOfWork.SaveChangesAsync(cancellationToken);
				if (affectedRows == 0)
					throw new Exception("Không thể lưu User vào DB");

				return _mapper.Map<UserDto>(user);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[EXCEPTION] Lỗi khi lưu User: {ex.Message}");
				throw;
			}
		}
	}
}
