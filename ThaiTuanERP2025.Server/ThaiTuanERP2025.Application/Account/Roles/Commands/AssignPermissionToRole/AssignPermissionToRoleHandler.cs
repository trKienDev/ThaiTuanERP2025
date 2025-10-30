using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Roles.Commands.AssignPermissionToRole
{
	public sealed class AssignPermissionToRoleHandler : IRequestHandler<AssignPermissionToRoleCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		public AssignPermissionToRoleHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(AssignPermissionToRoleCommand command, CancellationToken cancellationToken)
		{
			var role = await _unitOfWork.Roles.SingleOrDefaultAsync(
				q => q.Where(u => u.Id == command.RoleId),
				cancellationToken: cancellationToken
			) ?? throw new NotFoundException("Không tìm thấy role yêu cầu");

			var newRolePermissions = command.PermissionIds
			       .Select(pid => new RolePermission(role.Id, pid))
			       .ToList();

			await _unitOfWork.RolePermissions.ReplaceRangeAsync(
				q => q.RoleId == command.RoleId
				, newRolePermissions, cancellationToken
			);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
