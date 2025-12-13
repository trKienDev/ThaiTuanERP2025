using MediatR;
using System.ComponentModel.DataAnnotations;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Core.FileAttachments.Commands
{
	public sealed record CreateFileAttachmentCommand(
		Guid driveObjectId,
		string module,
		string document,
		string? documentId
	) : IRequest<Guid>;

	public sealed class CreateFileAttachmentCommandHandler : IRequestHandler<CreateFileAttachmentCommand, Guid>
	{
		private readonly IUnitOfWork _uow;
		private readonly ICurrentUserService _currentUser;
		public CreateFileAttachmentCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
		{
			_uow = uow;
			_currentUser = currentUser;
		}

		public async Task<Guid> Handle(CreateFileAttachmentCommand command, CancellationToken cancellationToken)
		{
			var userId = _currentUser.UserId ?? throw new ValidationException("User của bạn không hợp lệ");

			var existed = await _uow.FileAttachments.ExistAsync(
				q => q.DriveObjectId == command.driveObjectId,
				cancellationToken: cancellationToken
			);
			if (existed) throw new ValidationException("File này đã tồn tại");

			var entity = new FileAttachment(
				command.driveObjectId,
				command.module.Trim(),
				command.document.Trim(),
				userId.ToString()
			);

			await _uow.FileAttachments.AddAsync(entity, cancellationToken);
			await _uow.SaveChangesAsync(cancellationToken);

			return entity.Id;
		}
	}
}
