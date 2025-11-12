using AutoMapper;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Application.Core.Reminders
{
	public class ReminderMappingProfile : Profile
	{
		public ReminderMappingProfile() {
			CreateMap<UserReminder, UserReminderDto>();
		}
	}
}
