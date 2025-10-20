using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;

namespace ThaiTuanERP2025.Application.Notifications.Command.Notifications.MarkAllRead
{
	public sealed class MarkAllReadHandler : IRequestHandler<MarkAllReadCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUserService;
		public MarkAllReadHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
		}

		public async Task<Unit> Handle(MarkAllReadCommand request, CancellationToken cancellationToken)
		{
			var uid = _currentUserService.UserId;
			var items = await _unitOfWork.Notifications.ListAsync(
				q => q.Where(x => x.UserId == uid && !x.IsRead),
				cancellationToken: cancellationToken
			);

			foreach (var it in items)
				it.MarkRead();

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
