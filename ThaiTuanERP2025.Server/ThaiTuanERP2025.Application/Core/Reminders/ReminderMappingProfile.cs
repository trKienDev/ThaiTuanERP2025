using AutoMapper;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Shared.Utils;

namespace ThaiTuanERP2025.Application.Core.Reminders
{
	public class ReminderMappingProfile : Profile
	{
		public ReminderMappingProfile() {
			CreateMap<UserReminder, UserReminderDto>()
				.ForMember(dest => dest.DueAt, opt => opt.MapFrom(src => TimeZoneConverter.ToVietnamTime(src.DueAt)));
		}
	}
}
