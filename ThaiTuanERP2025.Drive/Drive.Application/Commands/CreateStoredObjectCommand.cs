using Drive.Application.Contracts;
using Drive.Application.Interfaces;
using Drive.Application.Shared.Interfaces;
using MediatR;
using System.ComponentModel.DataAnnotations;
using ThaiTuanERP2025.Domain.StoredFiles.Entities;

namespace Drive.Application.Commands
{
	public sealed record CreateStoredObjectCommand(RawObject Object, string Module, string Entity) : IRequest<Guid>;

	public sealed class CreateStoredObjectCommandHandler : IRequestHandler<CreateStoredObjectCommand, Guid>
	{
		private readonly IObjectStorage _storage;
		private readonly IUnitOfWork _uow;
		public CreateStoredObjectCommandHandler(IUnitOfWork uow, IObjectStorage storage)
		{
			_uow = uow;
			_storage = storage;
		}

		public async Task<Guid> Handle(CreateStoredObjectCommand command, CancellationToken cancellationToken) {
			if (command.Object is null || command.Object.Length <= 0)
				throw new ValidationException("Không có object được gửi lên");

			try
			{
				await _storage.EnsureReadyAsync(cancellationToken);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}

			var objectKey = BuildObjectKey(command.Object.FileName, command.Module, command.Entity);

			await using var stream = await command.Object.OpenReadStream(cancellationToken);
			var contentType = string.IsNullOrWhiteSpace(command.Object.ContentType) ? "application/octet-stream" : command.Object.ContentType!;

			await _storage.UploadAsync(objectKey, stream, contentType, cancellationToken);

			var entity = new StoredObject
			(
				objectKey,
				command.Object.FileName,
				contentType,
				command.Object.Length
			);

			await _uow.StoredObjects.AddAsync(entity, cancellationToken);
			await _uow.SaveChangesAsync(cancellationToken);

			return entity.Id;
		}

		private static string BuildObjectKey(string originalName, string module, string entity)
			=> $"{DateTime.UtcNow:yyyy/MM}/{module}/{entity}/" + $"{Guid.NewGuid():N}{Path.GetExtension(originalName)}";
	}
}
