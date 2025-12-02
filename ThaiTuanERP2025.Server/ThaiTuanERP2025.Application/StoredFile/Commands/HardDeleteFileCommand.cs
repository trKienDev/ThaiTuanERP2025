using MediatR;
using ThaiTuanERP2025.Application.Files.Interfaces;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Files.Commands
{
	public sealed record HardDeleteCommand(Guid id) : IRequest;
	public sealed class HardDeleteHandler : IRequestHandler<HardDeleteCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IFileStorage _storage;
		public HardDeleteHandler(IUnitOfWork unitOfWork, IFileStorage storage)
		{
			_unitOfWork = unitOfWork;
			_storage = storage;
		}

		public async Task<Unit> Handle(HardDeleteCommand request, CancellationToken cancellationToken)
		{
			var file = await _unitOfWork.StoredFiles.GetByIdAsync(request.id);
			if (file is null)
				return Unit.Value;

			await _storage.RemoveAsync(file.ObjectKey, cancellationToken);
			var ok = await _unitOfWork.StoredFiles.HardDeleteAsync(request.id, cancellationToken);
			if (ok)
				await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;

		}
	}
}
