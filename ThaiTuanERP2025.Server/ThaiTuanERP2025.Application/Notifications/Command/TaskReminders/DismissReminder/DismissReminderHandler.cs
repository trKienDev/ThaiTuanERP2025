using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;

namespace ThaiTuanERP2025.Application.Notifications.Command.TaskReminders.DismissReminder
{
	public class DismissReminderHandler : IRequestHandler<DismissReminderCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUserService;
		public DismissReminderHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
		}

		public async Task<Unit> Handle(DismissReminderCommand request, CancellationToken cancellationToken)
		{
			var uid = _currentUserService.UserId;
			var reminder = await _unitOfWork.TaskReminders.SingleOrDefaultIncludingAsync(
				a => a.Id == request.Id && a.UserId == uid,
				cancellationToken: cancellationToken
			);

			if (reminder == null) return Unit.Value;

			reminder.Resolve("Dismissed");
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
