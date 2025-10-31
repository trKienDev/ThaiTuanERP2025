using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Roles.Commands.DeleteRole
{
	public sealed class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		public DeleteRoleHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
		{
			var role = await _unitOfWork.Roles.GetByIdAsync(command.RoleId, cancellationToken)
				?? throw new NotFoundException("Không tìm thấy role yêu cầu");

			if (string.Equals(role.Name, "SuperAdmin", StringComparison.OrdinalIgnoreCase))
				throw new InvalidOperationException("Không thể xóa vai trò hệ thống SuperAdmin.");
			if (string.Equals(role.Name, "Admin", StringComparison.OrdinalIgnoreCase))
				throw new InvalidOperationException("Không thể xóa vai trò hệ thống Admin.");

			_unitOfWork.RolePermissions.RemoveRange(role.RolePermissions);
			_unitOfWork.UserRoles.RemoveRange(role.UserRoles);
			_unitOfWork.Roles.Delete(role);

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
