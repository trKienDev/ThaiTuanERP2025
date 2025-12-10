using MediatR;
using ThaiTuanERP2025.Application.Files.Contracts;
using ThaiTuanERP2025.Application.Files.Interfaces;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Files.Queries
{
	public sealed record DownloadStoredFileQuery(Guid FileId) : IRequest<StoredFileDownloadDto>;

	public sealed class DownloadStoredFileQueryHandler : IRequestHandler<DownloadStoredFileQuery, StoredFileDownloadDto>
	{
		private readonly IUnitOfWork _uow;
		private readonly IFileStorage _storageFile;
		public DownloadStoredFileQueryHandler(IUnitOfWork uow, IFileStorage storageFile) {
			_uow = uow;
			_storageFile = storageFile;
		}

		public async Task<StoredFileDownloadDto> Handle(DownloadStoredFileQuery request, CancellationToken cancellationToken)
		{
			var file = await _uow.StoredFiles.GetByIdAsync(request.FileId) ?? throw new NotFoundException("File không tồn tại");

			var stream = await _storageFile.OpenReadStreamAsync(file.ObjectKey, cancellationToken);

			return new StoredFileDownloadDto(stream, file.ContentType, file.FileName);
		}
	}
}
