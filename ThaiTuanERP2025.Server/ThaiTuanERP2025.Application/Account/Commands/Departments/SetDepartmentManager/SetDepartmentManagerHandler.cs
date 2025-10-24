using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Commands.Departments.SetDepartmentManager
{
	public sealed class SetDepartmentManagerHandler : IRequestHandler<SetDepartmentManagerCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		public SetDepartmentManagerHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(SetDepartmentManagerCommand command, CancellationToken cancellationToken) {
			// 1 ) Department có tồn tại
			var department = await _unitOfWork.Departments.GetByIdAsync(command.DepartmentId);
			if (department is null)
				throw new NotFoundException("Phòng ban không tồn tại");

			if(command.ManagerId is Guid mgrId) {
				var manager = await _unitOfWork.Users.GetByIdAsync(mgrId);
				if(manager is null)
					throw new NotFoundException("Người quản lý không tồn tại");
				if(manager.DepartmentId != department.Id) 
					throw new ConflictException("Người quản lý không thuộc phòng ban này");	
			}

			if(department.ManagerUserId == command.ManagerId)
				return Unit.Value;

			department.SetManager(command.ManagerId);

			var userRole = await _unitOfWork.Roles.SingleOrDefaultIncludingAsync(r => r.Name == "User");
			if (userRole == null)
				throw new NotFoundException("Vai trò 'User' không tồn tại.");

			var users = await _unitOfWork.Users.FindIncludingAsync(
				u => u.DepartmentId == department.Id &&
					u.UserRoles.Any(ur => ur.Role.Name == "User") &&
					u.Id != command.ManagerId,
				cancellationToken: cancellationToken
			);

			foreach(var u in users) {
				u.AssignManager(command.ManagerId);
			}

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	} 
}
