using AutoMapper;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Application.Core.Notifications
{
	public class NotificationMappingProfile : Profile
	{
		public NotificationMappingProfile() {
			CreateMap<UserNotification, UserNotificationDto>();
		}
	}
}
