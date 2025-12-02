using MediatR;
using ThaiTuanERP2025.Application.Files.Authorization.Interfaces;
using ThaiTuanERP2025.Application.Files.Contracts;
using ThaiTuanERP2025.Application.Files.Interfaces;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Files.Queries
{
	public sealed record DownloadStoredFileQuery(Guid FileId) : IRequest<StoredFileDownloadDto>;

	public sealed class DownloadStoredFileQueryHandler : IRequestHandler<DownloadStoredFileQuery, StoredFileDownloadDto>
	{
		private readonly IUnitOfWork _uow;
		private readonly IFileStorage _storageFile;
		private readonly ICurrentUserService _currentUser;
		private readonly IEnumerable<IStoredFilePermissionChecker> _checkers;
		public DownloadStoredFileQueryHandler(
			IUnitOfWork uow, IFileStorage storageFile, ICurrentUserService currentUser, IEnumerable<IStoredFilePermissionChecker> checkers
		) {
			_uow = uow;
			_storageFile = storageFile;
			_currentUser = currentUser;
			_checkers = checkers;
		}

		public async Task<StoredFileDownloadDto> Handle(DownloadStoredFileQuery request, CancellationToken cancellationToken = default)
		{
			var file = await _uow.StoredFiles.GetByIdAsync(request.FileId) ?? throw new NotFoundException("File không tồn tại");
			var userId = _currentUser.UserId ?? throw new UnauthorizedException("Không xác định được user");

			// Nếu file private → tìm checker phù hợp
			if (!file.IsPublic)
			{
				var checker = _checkers.FirstOrDefault(c => c.CanHandle(file.Module, file.Entity));
				if (checker == null) throw new ForbiddenException("Không có rule permission cho loại file này");

				var allowed = await checker.HasPermissionAsync(file, userId, cancellationToken);
				if (!allowed) throw new ForbiddenException("Bạn không có quyền xem file này");
			}

			var stream = await _storageFile.OpenReadStreamAsync(file.ObjectKey, cancellationToken);

			return new StoredFileDownloadDto(stream, file.ContentType, file.FileName, file.IsPublic);
		}
	}
}
