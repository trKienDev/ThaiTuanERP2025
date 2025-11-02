using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;

namespace ThaiTuanERP2025.Application.Alerts.Notifications.Commands.MarkRead
{
	public sealed class MarkReadCommandHandler : IRequestHandler<MarkReadCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUserService;
		public MarkReadCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
		}

		public async Task<Unit> Handle(MarkReadCommand request, CancellationToken cancellationToken)
		{
			var uid = _currentUserService.UserId;
			var notification = await _unitOfWork.Notifications.SingleOrDefaultIncludingAsync(x =>
				x.Id == request.Id && x.UserId == uid,
				cancellationToken: cancellationToken
			);

			if (notification is null) return Unit.Value;

			notification.MarkRead();
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
