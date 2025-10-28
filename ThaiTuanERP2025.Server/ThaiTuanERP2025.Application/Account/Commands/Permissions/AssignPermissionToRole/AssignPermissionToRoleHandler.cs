using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Commands.Permissions.AssignPermissionToRole
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

			var existingPermissionIds = role.RolePermissions.Select(rp => rp.PermissionId).ToList();

			var toAdd = request.PermissionIds.Except(existingPermissionIds).ToList();
			var toRemove = existingPermissionIds.Except(request.PermissionIds).ToList();

			if (toAdd.Any())
			{
				var newRolePermissions = toAdd.Select(pid => new RolePermission(role.Id, pid)).ToList();
				await _unitOfWork.RolePermissions.AddRangeAsync(newRolePermissions, cancellationToken);
			}

			if (toRemove.Any())
			{
				var removeEntities = role.RolePermissions.Where(rp => toRemove.Contains(rp.PermissionId)).ToList();
				_unitOfWork.RolePermissions.RemoveRange(removeEntities);
			}

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
