using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;

namespace ThaiTuanERP2025.Application.Core.Notifications.Queries
{
	public sealed record GetUnreadNotificationCountQuery() : IRequest<int>;

	public sealed class GetUnreadNotificationCountQueryHandler : IRequestHandler<GetUnreadNotificationCountQuery, int> {
		private readonly IUserNotificationReadRepository _notificationRepo;
		private readonly ICurrentUserService _currentUser;
		public GetUnreadNotificationCountQueryHandler(IUserNotificationReadRepository notificationRepo, ICurrentUserService currentUser)
		{
			_notificationRepo = notificationRepo;
			_currentUser = currentUser;
		}

		public async Task<int> Handle(GetUnreadNotificationCountQuery query, CancellationToken cancellationToken) {
			var uid = _currentUser.UserId;

			return await _notificationRepo.CountAsync(
				n => n.UserId == uid && !n.IsRead,
				cancellationToken: cancellationToken
			);
			
		}
	}
}
