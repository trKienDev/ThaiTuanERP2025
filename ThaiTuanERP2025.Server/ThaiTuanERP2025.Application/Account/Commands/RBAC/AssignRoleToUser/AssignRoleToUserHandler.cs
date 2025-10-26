using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Commands.RBAC.AssignRoleToUser
{
	public sealed class AssignRoleToUserHandler : IRequestHandler<AssignRoleToUserCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		public AssignRoleToUserHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
		{
			var user = await _unitOfWork.Users.SingleOrDefaultIncludingAsync(
				u => u.Id == request.UserId, 
				asNoTracking: false,
				cancellationToken: cancellationToken,
				r => r.UserRoles
			) ?? throw new InvalidOperationException("User not found.");

			var role = await _unitOfWork.Roles.GetByIdAsync(request.RoleId, cancellationToken) ?? throw new InvalidOperationException("Role not found.");

			var alreadyAssigned = user.UserRoles.Any(ur => ur.RoleId == role.Id);
			if (alreadyAssigned)
				throw new InvalidOperationException("Role already assigned to user.");

			var userRole = new UserRole(user.Id, role.Id);
			await _unitOfWork.UserRoles.AddAsync(userRole, cancellationToken);

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
