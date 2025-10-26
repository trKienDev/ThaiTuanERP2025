using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Commands.RBAC.AssignPermissionToRole
{
	public class AssignPermissionToRoleHandler : IRequestHandler<AssignPermissionToRoleCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		public AssignPermissionToRoleHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(AssignPermissionToRoleCommand request, CancellationToken cancellationToken)
		{
			var role = await _unitOfWork.Roles.SingleOrDefaultIncludingAsync(
				r => r.Id == request.RoleId,
				asNoTracking: false,
				cancellationToken: cancellationToken,
				r => r.RolePermissions
			) ?? throw new NotFoundException("Role not found.");

			var permission = await _unitOfWork.Permissions.GetByIdAsync(request.PermissionId, cancellationToken);
			if (permission == null)
				throw new InvalidOperationException("Permission not found.");

			var alreadyAssigned = role.RolePermissions.Any(rp => rp.PermissionId == permission.Id);
			if (alreadyAssigned)
				throw new InvalidOperationException("Permission already assigned to role.");

			var rolePermission = new RolePermission(role.Id, permission.Id);
			await _unitOfWork.RolePermissions.AddAsync(rolePermission, cancellationToken);

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
