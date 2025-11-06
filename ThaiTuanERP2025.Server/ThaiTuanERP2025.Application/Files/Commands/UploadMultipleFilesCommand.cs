using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Exceptions;
using ThaiTuanERP2025.Application.Files.Common;
using ThaiTuanERP2025.Domain.Files.Entities;

namespace ThaiTuanERP2025.Application.Files.Commands
{
	public sealed record UploadMultipleFilesCommand(List<RawFile> Files, string Module, string Entity, string? EntityId, bool IsPublic) : IRequest<List<UploadSingleFileResult>>;
	public sealed class UploadMultipleFilesHandler : IRequestHandler<UploadMultipleFilesCommand, List<UploadSingleFileResult>>
	{
		private readonly IFileStorage _storage;
		private readonly IUnitOfWork _unitOfWork;
		public UploadMultipleFilesHandler(IFileStorage storage, IUnitOfWork unitOfWork)
		{
			_storage = storage;
			_unitOfWork = unitOfWork;
		}

		public async Task<List<UploadSingleFileResult>> Handle(UploadMultipleFilesCommand request, CancellationToken cancellationToken)
		{
			if (request.Files is null || request.Files.Count == 0)
				throw new ValidationException("Không tìm thấy file upload");

			await _storage.EnsureReadyAsync(cancellationToken);
			var result = new List<UploadSingleFileResult>(request.Files.Count);

			foreach (var file in request.Files)
			{
				if (file is null || file.Length <= 0) continue;

				var key = BuildObjectKey(request.Module, request.Entity, file.FileName);
				await using var stream = await file.OpenReadStream(cancellationToken);
				var contentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType!;
				await _storage.UploadAsync(key, stream, contentType, cancellationToken);

				var bucketName = (_storage as IFileStorageInfo)?.BucketName ?? string.Empty;
				var entity = new StoredFile(
					bucketName, key, file.FileName, contentType, file.Length, request.Module, request.Entity, request.EntityId
				);

				await _unitOfWork.StoredFiles.AddAsync(entity);
				result.Add(new UploadSingleFileResult(entity.Id, key, entity.Size, entity.FileName, entity.ContentType));
			}

			await _unitOfWork.SaveChangesAsync();
			return result;
		}

		private static string BuildObjectKey(string module, string entity, string originalName)
			=> $"{module}/{entity}/{DateTime.UtcNow:yyyy/MM}/" + $"{Guid.NewGuid():N}{Path.GetExtension(originalName)}";
	}
}
