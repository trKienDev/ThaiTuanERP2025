using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;

namespace ThaiTuanERP2025.Application.Alerts.Notifications.Queries.GetAllNotifications
{
	public sealed class GetAllNotificationsQueryHandler : IRequestHandler<GetAllNotificationsQuery, IReadOnlyCollection<AppNotificationDto>>
	{
		private readonly INotificationReadRepository _notificationReadRepo;
		private readonly IMapper _mapper;
		private readonly ICurrentUserService _currentUserService;
		public GetAllNotificationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, INotificationReadRepository notificationReadRepo)
		{
			_notificationReadRepo = notificationReadRepo;
			_mapper = mapper;
			_currentUserService = currentUserService;
		}

		public async Task<IReadOnlyCollection<AppNotificationDto>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
		{
			var uid = _currentUserService.UserId;

			var query = _notificationReadRepo.Query().Where(n => n.UserId == uid);

			if (request.UnreadOnly)
				query = query.Where(n => !n.IsRead);

			var page = request.Page <= 0 ? 1 : request.Page;
			var pageSize = request.PageSize <= 0 || request.PageSize > 200 ? 30 : request.PageSize;

			var items = await _notificationReadRepo.ListProjectedAsync(
				q => q.Where(n => n.UserId == uid)
					.Where(n => !request.UnreadOnly || !n.IsRead)
					.OrderByDescending(n => n.CreatedDate)
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.ProjectTo<AppNotificationDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			);

			return items;
		}
	}
}
