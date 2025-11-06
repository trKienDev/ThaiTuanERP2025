using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;

namespace ThaiTuanERP2025.Application.Files.Commands
{
	public record SoftDeleteFileCommand(Guid id) : IRequest;
	public sealed class SoftDeleteFileHandler : IRequestHandler<SoftDeleteFileCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		public SoftDeleteFileHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(SoftDeleteFileCommand request, CancellationToken cancellationToken)
		{
			var ok = await _unitOfWork.StoredFiles.SoftDeleteAsync(request.id, cancellationToken);
			if (ok)
				await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
