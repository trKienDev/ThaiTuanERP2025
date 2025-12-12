using Drive.Application.Contracts;
using Drive.Application.Interfaces;
using Drive.Application.Shared.Exceptions;
using Drive.Application.Shared.Interfaces;
using MediatR;
using ThaiTuanERP2025.Domain.StoredFiles.Entities;

namespace Drive.Application.Commands
{
	public sealed record UploadSingleFileResult(Guid Id, string ObjectKey, long Size, string FileName, string ContentType);
	public sealed record UploadSingleObjectCommand(RawObject File, string Module, string Entity, string? EntityId, bool IsPublic) : IRequest<UploadSingleFileResult>;

	public sealed class UploadSingleFileHandler : IRequestHandler<UploadSingleObjectCommand, UploadSingleFileResult>
	{
		private readonly IObjectStorage _storage;
		private readonly IUnitOfWork _unitOfWork;

		public UploadSingleFileHandler(IObjectStorage storage, IUnitOfWork unitOfWork)
		{
			_storage = storage;
			_unitOfWork = unitOfWork;
		}

		public async Task<UploadSingleFileResult> Handle(UploadSingleObjectCommand request, CancellationToken cancellationToken)
		{
			if (request.File is null || request.File.Length <= 0)
				throw new NotFoundException("Không tìm thấy file upload");

			await _storage.EnsureReadyAsync(cancellationToken);

			var objectKey = BuildObjectKey(request.Module, request.Entity, request.File.FileName);

			await using var stream = await request.File.OpenReadStream(cancellationToken);
			var contentType = string.IsNullOrWhiteSpace(request.File.ContentType) ? "application/octet-stream" : request.File.ContentType!;

			// Upload to local storage
			await _storage.UploadAsync(objectKey, stream, contentType, cancellationToken);

			var bucketName = (_storage as IObjectStorageInfo)?.BucketName ?? string.Empty;
			var entity = new StoredObject
			(
				 bucketName,
				objectKey,
				request.File.FileName,
				contentType,
				request.File.Length,
				request.Module
			);

			await _unitOfWork.StoredFiles.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();

			return new UploadSingleFileResult(entity.Id, objectKey, entity.Size, entity.FileName, entity.ContentType);

		}

		private static string BuildObjectKey(string module, string entity, string originalName)
			=> $"{module}/{entity}/{DateTime.UtcNow:yyyy/MM}/" + $"{Guid.NewGuid():N}{Path.GetExtension(originalName)}";
	}
}
