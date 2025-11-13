using MediatR;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Account.Users.Commands
{
	public sealed record SetManagerCommand( Guid UserId, IReadOnlyList<Guid> ManagerIds, Guid? PrimaryManagerId ) : IRequest<Unit>;
	public sealed class SetManagerCommandHandler : IRequestHandler<SetManagerCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		public SetManagerCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(SetManagerCommand command, CancellationToken cancellationToken)
		{

			var user = await _unitOfWork.Users.GetByIdAsync(command.UserId, cancellationToken)
				?? throw new NotFoundException("Không tìm thấy người dùng yêu cầu.");

			var desired = command.ManagerIds.Distinct().Where(id => id != command.UserId).ToList();

			var currentManagers = await _unitOfWork.Users.GetActiveManagerAssignmentsAsync(command.UserId, cancellationToken);
			var currentIds = currentManagers.Select(x => x.ManagerId).ToHashSet();

			// Revoke
			foreach (var a in currentManagers.Where(x => !desired.Contains(x.ManagerId)))
				a.Revoke();

			// Add new
			var toAdd = desired.Where(id => !currentIds.Contains(id))
							.Select(mid => new UserManagerAssignment(command.UserId, mid, false))
							.ToList();
			if (toAdd.Count > 0) await _unitOfWork.Users.AddAssignmentsAsync(toAdd);

			// set primary
			var primaryId = command.PrimaryManagerId ?? desired.FirstOrDefault();
			foreach (var a in currentManagers.Where(x => x.RevokedAt == null))
				a.DemoteFromPrimary();

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
