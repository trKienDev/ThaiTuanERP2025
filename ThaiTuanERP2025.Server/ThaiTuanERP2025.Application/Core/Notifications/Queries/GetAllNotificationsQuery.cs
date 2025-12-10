using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Account.Users;
using ThaiTuanERP2025.Application.Core.Notifications.Contracts;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Shared.Utils;

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
		private readonly ICurrentUserService _currentUser;
		public GetAllNotificationsQueryHandler(IUserNotificationReadRepository notificationRepo, ICurrentUserService currentUser)
		{
			_notificationRepo = notificationRepo;
			_currentUser = currentUser;
		}

		public async Task<IReadOnlyCollection<UserNotificationDto>> Handle(GetAllNotificationsQuery query, CancellationToken cancellationToken) {
			var uid = _currentUser.UserId;

			var page = query.Page <= 0 ? 1 : query.Page;
			var pageSize = query.PageSize <= 0 || query.PageSize > 200 ? 30 : query.PageSize;

			return await _notificationRepo.ListProjectedAsync(
				q => q.Where(n => n.ReceiverId == uid && (!query.UnreadOnly || !n.IsRead))
					.OrderByDescending(n => n.CreatedAt)
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.Select(n => new UserNotificationDto
					{
						Id = n.Id,
						SenderId = n.SenderId,
						Sender = new UserBriefAvatarDto
						{
							Id = n.Sender.Id,
							FullName = n.Sender.FullName,
							AvatarFileId = n.Sender.AvatarFileId,
							AvatarFileObjectKey = n.Sender.AvatarFileObjectKey
						},
						Title = n.Title,
						Message = n.Message,
						Link = n.LinkUrl,
						LinkType = n.LinkType,
						TargetId = n.TargetId,
						Type = n.Type,
						CreatedAt = TimeZoneConverter.ToVietnamTime(n.CreatedAt),
						IsRead = n.IsRead
					}),
				cancellationToken: cancellationToken
			);
		}
	}
}
