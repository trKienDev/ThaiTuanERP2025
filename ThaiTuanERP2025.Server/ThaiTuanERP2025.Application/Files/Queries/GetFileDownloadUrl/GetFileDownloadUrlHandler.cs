using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Files.Queries.GetFileDownloadUrl
{
	public sealed class GetFileDownloadUrlHandler : IRequestHandler<GetFileDownloadUrlQuery, string>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IFileStorage _storage;
		public GetFileDownloadUrlHandler(IUnitOfWork unitOfWork, IFileStorage storage)
		{
			_unitOfWork = unitOfWork;
			_storage = storage;
		}

		public async Task<string> Handle(GetFileDownloadUrlQuery request, CancellationToken cancellationToken) {
			var file = await _unitOfWork.StoredFiles.GetByIdAsync(request.id)
				?? throw new NotFoundException("không tìm thấy file yêu cầu");

			// Bucket/expiry do hạ tầng quyết định
			return await _storage.GetPresignedGetUrlAsync(file.ObjectKey, cancellationToken);
		}
	}
}
