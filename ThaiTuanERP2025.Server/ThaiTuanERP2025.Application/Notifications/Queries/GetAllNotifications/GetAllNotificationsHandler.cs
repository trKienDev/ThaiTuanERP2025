using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Notifications.Dtos;

namespace ThaiTuanERP2025.Application.Notifications.Queries.GetAllNotifications
{
	public sealed class GetAllNotificationsHandler : IRequestHandler<GetAllNotificationsQuery, IReadOnlyCollection<AppNotificationDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ICurrentUserService _currentUserService;
		public GetAllNotificationsHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_currentUserService = currentUserService;
		}

		public async Task<IReadOnlyCollection<AppNotificationDto>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
		{
			var uid = _currentUserService.UserId;

			var query = _unitOfWork.Notifications.Query().Where(n => n.UserId == uid);

			if (request.UnreadOnly)
				query = query.Where(n => !n.IsRead);

			var page = request.Page <= 0 ? 1 : request.Page;
			var pageSize = request.PageSize <= 0 || request.PageSize > 200 ? 30 : request.PageSize;

			var items = await _unitOfWork.Notifications.ListProjectedAsync<AppNotificationDto>(q =>
				q.Where(n => n.UserId == uid)
				.Where(n => !request.UnreadOnly || !n.IsRead)
				.OrderByDescending(n => n.CreatedDate)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ProjectTo<AppNotificationDto>(_mapper.ConfigurationProvider),
			cancellationToken: cancellationToken);

			return items;
		}
	}
}
