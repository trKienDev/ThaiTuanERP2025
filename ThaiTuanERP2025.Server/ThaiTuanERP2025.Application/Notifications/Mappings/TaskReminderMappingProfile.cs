using AutoMapper;
using ThaiTuanERP2025.Application.Notifications.Dtos;
using ThaiTuanERP2025.Domain.Notifications;

namespace ThaiTuanERP2025.Application.Notifications.Mappings
{
	public sealed class TaskReminderMappingProfile : Profile
	{
		public TaskReminderMappingProfile()
		{
			CreateMap<TaskReminder, TaskReminderDto>()
				.ForCtorParam(nameof(TaskReminderDto.Id), o => o.MapFrom(s => s.Id))
				.ForCtorParam(nameof(TaskReminderDto.Title), o => o.MapFrom(s => s.Title))
				.ForCtorParam(nameof(TaskReminderDto.Message), o => o.MapFrom(s => s.Message))
				.ForCtorParam(nameof(TaskReminderDto.DueAt),o => o.MapFrom(s => new DateTimeOffset(DateTime.SpecifyKind(s.DueAt, DateTimeKind.Utc))))
				.ForCtorParam(nameof(TaskReminderDto.WorkflowInstanceId), o => o.MapFrom(s => s.WorkflowInstanceId))
				.ForCtorParam(nameof(TaskReminderDto.StepInstanceId), o => o.MapFrom(s => s.StepInstanceId));
		}
	}
}
