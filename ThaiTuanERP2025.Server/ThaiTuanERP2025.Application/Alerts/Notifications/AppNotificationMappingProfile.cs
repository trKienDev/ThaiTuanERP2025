using AutoMapper;
using ThaiTuanERP2025.Domain.Alerts.Entities;

namespace ThaiTuanERP2025.Application.Alerts.Notifications
{
	public sealed class AppNotificationMappingProfile : Profile
	{
		public AppNotificationMappingProfile() {
			CreateMap<AppNotification, AppNotificationDto>()
				.ForCtorParam(nameof(AppNotificationDto.Id), opt => opt.MapFrom(src => src.Id))
				.ForCtorParam(nameof(AppNotificationDto.Title), opt => opt.MapFrom(src => src.Title))
				.ForCtorParam(nameof(AppNotificationDto.Message), opt => opt.MapFrom(src => src.Message))
				.ForCtorParam(nameof(AppNotificationDto.Link), opt => opt.MapFrom(src => src.Link))
				.ForCtorParam(nameof(AppNotificationDto.CreatedAt), opt => opt.MapFrom(src => src.CreatedDate)) // từ AuditableEntity
				.ForCtorParam(nameof(AppNotificationDto.IsRead), opt => opt.MapFrom(src => src.IsRead));
		}
	}
}
