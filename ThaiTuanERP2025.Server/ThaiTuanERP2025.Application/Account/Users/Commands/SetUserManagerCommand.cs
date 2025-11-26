using MediatR;
using ThaiTuanERP2025.Application.Account.Users.Services;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Account.Users.Commands
{
	public sealed record SetManagerCommand( Guid UserId, IReadOnlyList<Guid> ManagerIds, Guid? PrimaryManagerId ) : IRequest<Unit>;
	public sealed class SetManagerCommandHandler : IRequestHandler<SetManagerCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserManagerService _managerSerivce;
		public SetManagerCommandHandler(IUnitOfWork unitOfWork, IUserManagerService managerService)
		{
			_unitOfWork = unitOfWork;
			_managerSerivce = managerService;
		}

		public async Task<Unit> Handle(SetManagerCommand command, CancellationToken cancellationToken)
		{

			var desired = command.ManagerIds?.Distinct()?.ToList() ?? new List<Guid>();
			var primary = command.PrimaryManagerId ?? desired.FirstOrDefault();

			await _managerSerivce.ReplaceAsync(command.UserId, desired, primary, cancellationToken);

			// Commit ở đây vì đây là command độc lập
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
