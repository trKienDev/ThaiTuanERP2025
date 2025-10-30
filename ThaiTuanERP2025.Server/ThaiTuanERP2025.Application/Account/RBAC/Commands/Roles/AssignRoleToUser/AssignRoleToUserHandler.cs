using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Account.Commands.Users.UpdateUserAvatarFileId;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.RBAC.Commands.Roles.AssignRoleToUser
{
	public class AssignRoleToUserHandler : IRequestHandler<AssignRoleToUserCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public AssignRoleToUserHandler(IUnitOfWork unitOfWork, IMapper mapper) {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<Unit> Handle(AssignRoleToUserCommand command, CancellationToken cancellationToken) {
			var user = await _unitOfWork.Users.SingleOrDefaultAsync(
				q => q.Where(u => u.Id == command.UserId),
				cancellationToken: cancellationToken
			) ?? throw new KeyNotFoundException("User not found");

			var newUserRoles = command.RoleIds
			    .Select(roleId => new UserRole(user.Id, roleId))
			    .ToList();

			await _unitOfWork.UserRoles.ReplaceRangeAsync(
				q => q.UserId == user.Id, 
				newUserRoles, cancellationToken
			);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
