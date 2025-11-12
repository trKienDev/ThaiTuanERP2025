using AutoMapper;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Application.Core.Notifications
{
	public class NotificationMappingProfile : Profile
	{
		public NotificationMappingProfile() {
			CreateMap<UserNotification, UserNotificationDto>()
				.ForMember(dest => dest.Link, opt => opt.MapFrom(src => src.LinkUrl))
				.ForMember(
					dest => dest.CreatedAt,
					opt => opt.MapFrom(src => TimeZoneInfo.ConvertTimeFromUtc(
						src.CreatedAt,
						TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")
					)
				)
			); 
		}
	}
}
