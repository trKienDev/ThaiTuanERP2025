﻿using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Files.Entities;

namespace ThaiTuanERP2025.Application.Files.Commands.UploadFile
{
	public sealed class UploadFileHandler : IRequestHandler<UploadFileCommand, UploadFileResult>
	{
		private readonly IFileStorage _storage;
		private readonly IUnitOfWork _unitOfWork;

		public UploadFileHandler(IFileStorage storage, IUnitOfWork unitOfWork) {
			_storage = storage;
			_unitOfWork = unitOfWork;
		}

		public async Task<UploadFileResult> Handle(UploadFileCommand request, CancellationToken cancellationToken) {
			if (request.File is null || request.File.Length <= 0)
				throw new ValidationException("File", "Không tìm thấy file upload");

			await _storage.EnsureReadyAsync(cancellationToken);

			var objectKey = BuildObjectKey(request.Module, request.Entity, request.EntityId, request.File.FileName);

			await using var stream = await request.File.OpenReadStream(cancellationToken);
			var contentType = string.IsNullOrWhiteSpace(request.File.ContentType) ? "application/octet-stream" : request.File.ContentType!;

			// Upload to local storage
			await _storage.UploadAsync(objectKey, stream, contentType, cancellationToken);

			var bucketName = ( _storage as IFileStorageInfo)?.BucketName ?? string.Empty;
			var entity = new StoredFile
			{
				Id = Guid.NewGuid(),
				Bucket = bucketName,           // (giữ trường nếu schema còn; hạ tầng quản bucket → có thể để trống/ghi tên cố định khi save)
				ObjectKey = objectKey,
				FileName = request.File.FileName,
				ContentType = contentType,
				Size = request.File.Length,
				IsPublic = request.IsPublic,
				Module = request.Module,
				Entity = request.Entity,
				EntityId = request.EntityId
			};

			await _unitOfWork.StoredFiles.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();

			return new UploadFileResult(entity.Id, objectKey, entity.Size, entity.FileName, entity.ContentType);

		}

		private static string BuildObjectKey(string module, string entity, string? entityId, string originalName)
			=> $"{module}/{entity}/{DateTime.UtcNow:yyyy/MM}/" +
			   $"{Guid.NewGuid():N}{Path.GetExtension(originalName)}";
	}
}
