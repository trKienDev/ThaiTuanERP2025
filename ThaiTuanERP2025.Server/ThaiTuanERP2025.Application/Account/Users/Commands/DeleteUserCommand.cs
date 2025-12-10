using MediatR;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Account.Users.Commands
{
	public sealed record DeleteUserCommand(Guid UserId) : IRequest<Unit>;
	public sealed class HardDeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
	{
		private readonly IUnitOfWork _uow;
		public HardDeleteUserCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public async Task<Unit> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
		{
			Guard.AgainstNullOrEmptyGuid(command.UserId, nameof(command.UserId));
			var userExisted = await _uow.Users.GetByIdAsync(command.UserId, cancellationToken)
				?? throw new NotFoundException("Không tìm thấy user");
			userExisted.DeletePermanently();

			_uow.Users.Delete(userExisted);
			await _uow.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
