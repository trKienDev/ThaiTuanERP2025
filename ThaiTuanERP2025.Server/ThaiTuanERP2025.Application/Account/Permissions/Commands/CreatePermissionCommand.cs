using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Exceptions;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Application.Account.Permissions.Commands
{
	public sealed record CreatePermissionCommand(PermissionRequest Request) : IRequest<Unit>;
	public sealed class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, Unit>
	{
		private readonly IUnitOfWork _uow;
		public CreatePermissionCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public async Task<Unit> Handle(CreatePermissionCommand command, CancellationToken cancellationToken)
		{
			var request = command.Request;

			var name = request.Name.Trim();
			Guard.AgainstNullOrWhiteSpace(name, nameof(request.Name));

			var code = request.Code.Trim();
			Guard.AgainstNullOrWhiteSpace(code, nameof(request.Code));

			var description = request.Description.Trim();

			var existed = await _uow.Permissions.ExistAsync(
				p => p.Name == name || p.Code == code,
				cancellationToken
			);
			if (existed) throw new NotFoundException("Quyền này đã tồn tại");

			var newPermission = new Permission(name, code, description);
			await _uow.Permissions.AddAsync(newPermission, cancellationToken);
			await _uow.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
