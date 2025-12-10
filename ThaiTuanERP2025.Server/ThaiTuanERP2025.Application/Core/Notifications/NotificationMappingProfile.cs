using AutoMapper;
using ThaiTuanERP2025.Application.Core.Notifications.Contracts;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Shared.Utils;

namespace ThaiTuanERP2025.Application.Core.Notifications
{
	public class NotificationMappingProfile : Profile
	{
		public NotificationMappingProfile() {
			CreateMap<UserNotification, UserNotificationDto>()
				.ForMember(dest => dest.Link, opt => opt.MapFrom(src => src.LinkUrl))
				.ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.Sender))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => TimeZoneConverter.ToVietnamTime(src.CreatedAt))); 
		}
	}
}
