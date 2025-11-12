using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Exceptions;

namespace ThaiTuanERP2025.Application.Core.Notifications.Queries
{
	public sealed record GetAllNotificationsQuery
	(
		bool UnreadOnly = false,
		int Page = 1,
		int PageSize = 20
	) : IRequest<IReadOnlyCollection<UserNotificationDto>>;

	public sealed class GetAllNotificationsQueryHandler : IRequestHandler<GetAllNotificationsQuery, IReadOnlyCollection<UserNotificationDto>> {
		private readonly IUserNotificationReadRepository _notificationRepo;
		private readonly IMapper _mapper;
		private readonly ICurrentUserService _currentUser;
		public GetAllNotificationsQueryHandler(IUserNotificationReadRepository notificationRepo, IMapper mapper, ICurrentUserService currentUser)
		{
			_notificationRepo = notificationRepo;
			_mapper = mapper;
			_currentUser = currentUser;
		}

		public async Task<IReadOnlyCollection<UserNotificationDto>> Handle(GetAllNotificationsQuery query, CancellationToken cancellationToken) {
			var uid = _currentUser.UserId;

			var page = query.Page <= 0 ? 1 : query.Page;
			var pageSize = query.PageSize <= 0 || query.PageSize > 200 ? 30 : query.PageSize;

			return await _notificationRepo.ListProjectedAsync(
				q => q.Where(n => n.UserId == uid && !query.UnreadOnly || !n.IsRead)
					.OrderByDescending(n => n.CreatedAt)
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.ProjectTo<UserNotificationDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			);

		}
	}
}
