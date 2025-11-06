using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Permissions.Commands
{
	public sealed record AssignPermissionToRoleCommand(Guid RoleId, List<Guid> PermissionIds) : IRequest<Unit>;
	public sealed class AssignPermissionToRoleCommandHandler : IRequestHandler<AssignPermissionToRoleCommand, Unit>
	{
		private readonly IUnitOfWork _uow;
		public AssignPermissionToRoleCommandHandler(IUnitOfWork uow) => _uow = uow;

		public async Task<Unit> Handle(AssignPermissionToRoleCommand command, CancellationToken cancellationToken)
		{
			var role = await _uow.Roles.SingleOrDefaultIncludingAsync(
				r => r.Id == command.RoleId,
				asNoTracking: false,
				cancellationToken: cancellationToken,
				r => r.RolePermissions
			) ?? throw new NotFoundException("Không tìm thấy role");

			var existingPermisionIds = role.RolePermissions.Select(rp => rp.PermissionId).ToHashSet();

			var toAdd = command.PermissionIds.Except(existingPermisionIds).ToList();
			var toRemove = existingPermisionIds.Except(command.PermissionIds).ToList();

			if (toAdd.Any())
			{
				foreach (var permission in toAdd)
				{
					role.AssignPermission(permission);
				}
			}

			if (toRemove.Any())
			{
				foreach (var permission in toRemove)
				{
					role.RemovePermission(permission);
				}
			}

			await _uow.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
