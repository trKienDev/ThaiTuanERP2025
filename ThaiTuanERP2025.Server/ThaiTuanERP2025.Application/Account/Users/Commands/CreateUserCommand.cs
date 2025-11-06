using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Security;
using ThaiTuanERP2025.Application.Exceptions;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Users.Commands
{
	public sealed record CreateUserCommand(
		string Fullname,
		string Username,
		string EmployeeCode,
		string Password,
		Guid RoleId,
		string Position,
		Guid DepartmentId,
		string? Email,
		string? Phone
	) : IRequest<Unit>;

	public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IPasswordHasher _passwordHasher;

		public CreateUserCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
		{
			_unitOfWork = unitOfWork;
			_passwordHasher = passwordHasher;
		}

		public async Task<Unit> Handle(CreateUserCommand command, CancellationToken cancellationToken)
		{
			var department = await _unitOfWork.Departments.GetByIdAsync(command.DepartmentId, cancellationToken)
				?? throw new NotFoundException("Phòng ban không tồn tại");

			var role = await _unitOfWork.Roles.ExistAsync(q => q.Id == command.RoleId, cancellationToken);
			if (!role) throw new NotFoundException("Vai trò không tồn tại");

			var user = new User(
				command.Fullname,
				command.Username,
				command.EmployeeCode,
				_passwordHasher.Hash(command.Password),
				command.Position,
				command.DepartmentId,
				command.Email,
				command.Phone
			);
			user.AssignRole(command.RoleId);

			var primaryManagerId = department.Managers.FirstOrDefault(m => m.IsPrimary)?.UserId;
			if (primaryManagerId.HasValue)
			{
				// Gán manager trực tiếp lên User (domain event: UserManagerAssignedEvent)
				user.AssignManager(primaryManagerId.Value);

				// (Tuỳ kiến trúc) Nếu bạn vẫn duy trì bảng quan hệ để report/lịch sử:
				var assignment = new UserManagerAssignment(
				    userId: user.Id,
				    managerId: primaryManagerId.Value,
				    isPrimary: true
				);
				await _unitOfWork.UserManagerAssignments.AddAsync(assignment, cancellationToken);
			}

			await _unitOfWork.Users.AddAsync(user, cancellationToken);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
