using Drive.Application.Contracts;
using Drive.Application.Shared.Interfaces;
using MediatR;
using ThaiTuanERP2025.Domain.StoredFiles.Entities;

namespace Drive.Application.Commands
{
	public sealed record CreateStoredObjectCommand(StoredObjectPayload Payload) : IRequest<Guid>;

	public sealed class CreateStoredObjectCommandHandler : IRequestHandler<CreateStoredObjectCommand, Guid>
	{
		private readonly IUnitOfWork _uow;
		public CreateStoredObjectCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public async Task<Guid> Handle(CreateStoredObjectCommand command, CancellationToken cancellationToken) {
			var payload = command.Payload;

			// 1. Create domain entity
			var storedObject = new StoredObject(
				bucket: payload.Bucket,
				objectKey: payload.ObjectKey,
				fileName: payload.FileName,
				contentType: payload.ContentType,
				size: payload.Size
			);

			// 2. Persist
			await _uow.StoredFiles.AddAsync(storedObject, cancellationToken);
			await _uow.SaveChangesAsync(cancellationToken);

			// 3. Return ID
			return storedObject.Id;
		}
	}
}
