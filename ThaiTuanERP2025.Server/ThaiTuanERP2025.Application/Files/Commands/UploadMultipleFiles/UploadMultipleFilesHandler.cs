using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Files.Commands.UploadFile;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Files.Entities;

namespace ThaiTuanERP2025.Application.Files.Commands.UploadMultipleFiles
{
	public sealed class UploadMultipleFilesHandler : IRequestHandler<UploadMultipleFilesCommand, List<UploadFileResult>>
	{
		private readonly IFileStorage _storage;
		private readonly IUnitOfWork _unitOfWork;
		public UploadMultipleFilesHandler(IFileStorage storage, IUnitOfWork unitOfWork)
		{
			_storage = storage;
			_unitOfWork = unitOfWork;
		}

		public async Task<List<UploadFileResult>> Handle(UploadMultipleFilesCommand request, CancellationToken cancellationToken) {
			if (request.Files is null || request.Files.Count == 0)
				throw new ValidationException("Files", "Không tìm thấy file upload");

			await _storage.EnsureReadyAsync(cancellationToken);
			var result = new List<UploadFileResult>(request.Files.Count);

			foreach (var file in request.Files)
			{
				if (file is null || file.Length <= 0) continue;

				var key = BuildObjectKey(request.Module, request.Entity, request.EntityId, file.FileName);
				await using var stream = await file.OpenReadStream(cancellationToken);
				var contentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType!;
				await _storage.UploadAsync(key, stream, contentType, cancellationToken);

				var bucketName = (_storage as IFileStorageInfo)?.BucketName ?? string.Empty;
				var entity = new StoredFile
				{
					Id = Guid.NewGuid(),
					Bucket = bucketName,
					ObjectKey = key,
					FileName = file.FileName,
					ContentType = contentType,
					Size = file.Length,
					IsPublic = request.IsPublic,
					Module = request.Module,
					Entity = request.Entity,
					EntityId = request.EntityId
				};

				await _unitOfWork.StoredFiles.AddAsync(entity);
				result.Add(new UploadFileResult(entity.Id, key, entity.Size, entity.FileName, entity.ContentType));
			}

			await _unitOfWork.SaveChangesAsync();
			return result;
		}

		private static string BuildObjectKey(string module, string entity, string? entityId, string originalName)
			=> $"{module}/{entity}/{DateTime.UtcNow:yyyy/MM}/" +
			   $"{Guid.NewGuid():N}{Path.GetExtension(originalName)}";
	}
}
