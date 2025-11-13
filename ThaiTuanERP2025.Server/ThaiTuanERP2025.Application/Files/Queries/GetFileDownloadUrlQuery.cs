using MediatR;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Files.Queries
{
	public sealed record GetFileDownloadUrlQuery(Guid id) : IRequest<string>;
	public sealed class GetFileDownloadUrlHandler : IRequestHandler<GetFileDownloadUrlQuery, string>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IFileStorage _storage;
		public GetFileDownloadUrlHandler(IUnitOfWork unitOfWork, IFileStorage storage)
		{
			_unitOfWork = unitOfWork;
			_storage = storage;
		}

		public async Task<string> Handle(GetFileDownloadUrlQuery request, CancellationToken cancellationToken)
		{
			var file = await _unitOfWork.StoredFiles.GetByIdAsync(request.id)
				?? throw new NotFoundException("không tìm thấy file yêu cầu");

			// Bucket/expiry do hạ tầng quyết định
			return await _storage.GetPresignedGetUrlAsync(file.ObjectKey, cancellationToken);
		}
	}
}
